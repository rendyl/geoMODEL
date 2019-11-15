using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematic : MonoBehaviour
{
    public List<Vector3> posJoint;
    [SerializeField]
    List<float> distJoint;
    [SerializeField]
    List<float> angleJoint;
    public int indexSelected = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < posJoint.Count - 1; i++)
        {
            angleJoint.Add(90);
            distJoint.Add(Vector3.Distance(posJoint[i], posJoint[i+1]));
        }
    }

    void forwardBackward()
    {
        for (int i = posJoint.Count - 2; i >= 0; i--)
        {
            float Ri = Vector3.Distance(posJoint[i], posJoint[i + 1]);
            float Lambdai = distJoint[i] / Ri;
            posJoint[i] = (1 - Lambdai) * posJoint[i + 1] + Lambdai * posJoint[i];

            if (i < posJoint.Count - 2)
            {
                float maxAngle = angleJoint[i] * Mathf.Deg2Rad; 
                Vector3 vec1 = (posJoint[i] - posJoint[i + 1]).normalized;
                Vector3 vec2 = (posJoint[i + 1] - posJoint[i + 2]).normalized;
                
                if (Vector3.Dot(vec2, vec1) < Mathf.Cos(maxAngle))
                {
                    Vector3 dir = Vector3.RotateTowards(vec2, vec1, maxAngle, 0);
                    dir.z = 0;
                    posJoint[i] = posJoint[i+1] + distJoint[i] * dir;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            for(int i = 0; i < 3; i++)
            {
                Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                posJoint[posJoint.Count - 1] = new Vector3(pz.x, pz.y, 0);

                Vector3 b = posJoint[0];

                // forward
                forwardBackward();

                posJoint[0] = b;
                posJoint.Reverse();
                distJoint.Reverse();

                forwardBackward();

                posJoint.Reverse();
                distJoint.Reverse();
            }
        }
    }

    void OnDrawGizmosSelected()
    { 
        if(Application.isPlaying)
        {
            Gizmos.color = Color.gray;
            for (int i = 0; i < posJoint.Count - 1; i++)
            {
                Vector3 vecActual = posJoint[i + 1] - posJoint[i];

                Gizmos.color = Color.gray;
                Gizmos.DrawLine(posJoint[i], posJoint[i + 1]);
            }

            Gizmos.color = Color.gray;
            for (int i = 0; i < posJoint.Count; i++)
            {
                if (i == posJoint.Count - 1)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(posJoint[i], 0.08f);
                    Gizmos.color = Color.gray;
                }
                else
                {
                    Gizmos.DrawSphere(posJoint[i], 0.08f);
                }
            }
        }
    }
}

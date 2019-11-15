using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite : MonoBehaviour
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 v0;
    public Vector3 v1;

    public int nbPts;
    List<Vector3> pts;

    void hermite()
    {
        pts.Clear();
        for (int i = 0; i < nbPts; i++)
        {
            float u = (i / ((float)nbPts - 1f));
            float f1 = 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
            float f2 = -2 * Mathf.Pow(u, 3) + 3 * Mathf.Pow(u, 2);
            float f3 = Mathf.Pow(u, 3) - 2 * Mathf.Pow(u, 2) + u;
            float f4 = Mathf.Pow(u, 3) - Mathf.Pow(u, 2);
            pts.Add(f1 * p0 + f2*p1 + f3*v0 + f4*v1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pts = new List<Vector3>();
        hermite();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            hermite();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < pts.Count - 1; i++)
            {
                Gizmos.DrawLine(pts[i], pts[i + 1]);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pts[0], 0.01f);
            Gizmos.DrawSphere(pts[pts.Count - 1], 0.01f);
        }
    }
}

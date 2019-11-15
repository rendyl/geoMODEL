using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChaikin : MonoBehaviour
{
    [Range(1, 20)]
    public int nbDiv;

    public List<Vector3> listVec;
    public List<Vector3> listVecChaikin;

    public static List<Vector3> chaikin(List<Vector3> listVec)
    {
        var output = new List<Vector3>();

        for (var i = 0; i < listVec.Count; i++)
        {
            var p0 = listVec[i];
            var p1 = listVec[(i + 1)% listVec.Count];

            var p0x = p0.x;
            var p0y = p0.z;

            var p1x = p1.x;
            var p1y = p1.z;

            var qx = 0.75f * p0x + 0.25f * p1x;
            var qy = 0.75f * p0y + 0.25f * p1y;

            var Q = new Vector3(qx, 0, qy);

            var rx = 0.25f * p0x + 0.75f * p1x;
            var ry = 0.25f * p0y + 0.75f * p1y;
            var R = new Vector3(rx, 0, ry);

            output.Add(Q);
            output.Add(R);
        }

        return output;
    }

    public void Start()
    {
        listVec = new List<Vector3>();
        for (int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                if (i%2 == 0) listVec.Add(new Vector3(i, 0, j));
                else listVec.Add(new Vector3(i, 0, 1 - j));
            }
        }
    }

    public void Update()
    {
        listVecChaikin = listVec;
        for (int i = 0; i < nbDiv; i++) listVecChaikin = chaikin(listVecChaikin);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for(int k = 0; k < listVec.Count; k++)
        {
            Debug.Log("k" + k + "k+1" + ((k + 1) % listVec.Count));
            Gizmos.DrawLine(10*listVec[k], 10*listVec[(k + 1)%listVec.Count]);
        }

        Gizmos.color = Color.cyan;
        for (int k = 0; k < listVecChaikin.Count; k++)
        {
            Gizmos.DrawLine(10*listVecChaikin[k], 10*listVecChaikin[(k + 1) % listVecChaikin.Count]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    List<Vector3> pts;
    public List<Vector3> ptControle;
    public int nbPtsControle = 4;
    public int nbPts = 10;
    public int indexSelected = 0;   

    static int Factoriel(int n)
    {
        return n > 1 ? n * Factoriel(n - 1) : 1;
    }

    float bernstein(int i, int n, float t)
    {
        return (Factoriel(n) / (Factoriel(i) * (Factoriel(n - i)))) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i);
    }

    void calculBezier()
    {
        pts.Clear(); 
        for (int k = 0; k < nbPts; k++)
        {
            float t = k / ((float)nbPts - 1f);
            Vector3 sum = new Vector3();
            for (int i = 0; i < nbPtsControle; i++)
            {
                sum += ptControle[i] * bernstein(i, nbPtsControle - 1, t);
            }
            pts.Add(sum);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pts = new List<Vector3>();

        calculBezier();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            calculBezier();
        }

        if(Input.GetKey(KeyCode.Z))
        {
            ptControle[indexSelected]  += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            ptControle[indexSelected] += new Vector3(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ptControle[indexSelected] -= new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ptControle[indexSelected] -= new Vector3(0.1f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) indexSelected = 3-0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) indexSelected = 3-1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) indexSelected = 3-2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) indexSelected = 3-3;
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
            for (int i = 0; i < nbPtsControle; i++)
            {
                if (i < nbPtsControle - 1)
                {
                    Gizmos.DrawLine(ptControle[i], ptControle[i + 1]);
                }
                if (i == indexSelected) Gizmos.color = Color.red;
                Gizmos.DrawSphere(ptControle[i], 0.08f);
                Gizmos.color = Color.cyan;
            }
        }
    }
}
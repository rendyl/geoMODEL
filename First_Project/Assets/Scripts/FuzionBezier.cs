using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzionBezier : MonoBehaviour
{
    public Bezier b1;
    public Bezier b2;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            fusionBezier(b1, b2);
        }
    }

    public Bezier fusionBezier(Bezier b1, Bezier b2)
    {
        Bezier b = new Bezier();
        b.nbPts = b1.nbPts + b2.nbPts;
        b.nbPtsControle = b1.nbPtsControle + b2.nbPtsControle - 1;

        for (int i = 0; i < b1.nbPts; i++)
        {
            b.ptControle.Add(b1.ptControle[i]);
        }
        b.ptControle.Add((2 * b1.ptControle[b1.nbPts - 1] - b1.ptControle[b1.nbPts - 2]));
        for (int i = 1; i < b2.nbPtsControle; i++)
        {
            b.ptControle.Add(b2.ptControle[i]);
        }

        Instantiate(b, new Vector3(0, 0, 0), Quaternion.identity);

        return b;

    }
}

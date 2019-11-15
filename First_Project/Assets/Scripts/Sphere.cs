using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material mat;

    [Range(1, 100)]
    public float rayon = 1;
    [Range(1, 100)]
    public int nbParalleles = 4;
    [Range(1, 100)]
    public int nbMeridiens = 4;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();   
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[nbParalleles * (nbMeridiens + 1) + 2];
        int[] triangles = new int[3 * 2 * (nbParalleles * nbMeridiens + 2)];
        List<int> triangl = new List<int>();

        vertices[0] = new Vector3(0, rayon, 0);
        // Debug.Log("Vertices : " + (0) + " (" + (0) + ", " + (rayon) + ")");
        for (int j = 0; j < nbParalleles; j++)
        {
            float phiJ = ((j + 1) * Mathf.PI) / (nbParalleles + 1);
            float cosJ = Mathf.Cos(phiJ);
            float sinJ = Mathf.Sin(phiJ);

            // Debug.Log("phiJ : " + (j) + " (" + (((j + 1) * 180) / (nbParalleles + 1)) + ")");

            for (int i = 0; i <= nbMeridiens; i++)
            {
                float thetaI = ((i % nbMeridiens) * (2 * Mathf.PI) / nbMeridiens);
                float cosI = Mathf.Cos(thetaI);
                float sinI = Mathf.Sin(thetaI);

                // Debug.Log("thetaI : " + (i) + " (" + (((i % nbMeridiens) * (360) / nbMeridiens)) + ")");

                vertices[i + j * (nbMeridiens + 1) + 1] = new Vector3(rayon * sinJ * cosI, rayon * cosJ, rayon * sinI * sinJ);
            }
        }
        vertices[nbParalleles * (nbMeridiens + 1) + 1] = new Vector3(0, -rayon, 0);
        // Debug.Log("Vertices : " + (nbParalleles * nbMeridiens + 1) + " (" + (0) + ", " + (0) + ", " + (-rayon) + ")");

        // Top
        for (int i = 0; i < nbMeridiens; i++)
        {
            triangl.Add(i + 2);
            triangl.Add(i + 1);
            triangl.Add(0);
        }

        // Middle
        for (int j = 0; j < nbParalleles - 1; j++)
        {
            for (int i = 0; i < nbMeridiens; i++)
            {
                triangl.Add(i + j * (nbMeridiens + 1) + 1);
                triangl.Add(i + j * (nbMeridiens + 1) + 2);
                triangl.Add(i + j * (nbMeridiens + 1) + 1 + nbMeridiens + 2);

                triangl.Add(i + j * (nbMeridiens + 1) + 1);
                triangl.Add(i + j * (nbMeridiens + 1) + 1 + nbMeridiens + 2);
                triangl.Add(i + j * (nbMeridiens + 1) + 1 + nbMeridiens + 1);
            }
        }

        // Bot
        for (int i = 0; i < nbMeridiens; i++)
        {
            triangl.Add(nbParalleles * (nbMeridiens + 1) + 1);
            triangl.Add(nbParalleles * (nbMeridiens + 1) + 1 - (i + 2));
            triangl.Add(nbParalleles * (nbMeridiens + 1) + 1 - (i + 1));
        }

        for (int i = 0; i < triangl.Count; i++)
        {
            triangles[i] = triangl[i];
            // Debug.Log("Triangl[i] : " + triangl[i]);
        }

        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}

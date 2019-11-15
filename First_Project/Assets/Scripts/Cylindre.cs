using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre : MonoBehaviour
{
    public Material mat;

    [Range(1, 100)]
    public float rayon = 2;
    [Range(0, 100)]
    public float petitrayon = 2;
    [Range(1, 100)]
    public float hauteur = 10;
    [Range(1, 100)]
    public int nbMeridiens = 8;
    [Range(1, 100)]
    public int nbEtages = 3;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[nbEtages * nbMeridiens + 2];
        int[] triangles = new int[3 * 2 * nbEtages * nbMeridiens];
        List<int> triangl = new List<int>();

        for (int i = 0; i < nbEtages * nbMeridiens; i = i + nbEtages)
        {
            float thetaI = i * ((Mathf.PI / 180) * 360) / (nbEtages * nbMeridiens);
            Debug.Log("Theta : " + (i) + " (" + ((i * (360)) / (nbEtages * nbMeridiens)) + ")");

            for (int k = 0; k < nbEtages; k++)
            {
                Debug.Log("Vertices : " + (i + k) + " (" + (rayon * Mathf.Cos(thetaI)) + ", " + ((-hauteur / 2) + k * (hauteur / (nbEtages - 1))) + ")");
                float radius = rayon - (rayon - petitrayon) * ((float)k / (nbEtages + 1));
                vertices[i+k] = new Vector3(radius * Mathf.Cos(thetaI), radius * Mathf.Sin(thetaI), (-hauteur / 2) + k*(hauteur/(nbEtages-1)));
            }
        }
        Debug.Log("Vertices : " + (nbEtages * nbMeridiens) + " (" + (0) + ", " + (0) + ")");
        Debug.Log("Vertices : " + (nbEtages * nbMeridiens + 1) + " (" + (0) + ", " + (0) + ")");
        vertices[nbEtages * nbMeridiens] = new Vector3(0, 0, -hauteur / 2);
        vertices[nbEtages * nbMeridiens + 1] = new Vector3(0, 0, +hauteur / 2);

        // Middle
        for (int i = 0; i < nbEtages * nbMeridiens; i = i + nbEtages)
        {
            // Debug.Log("i : " + i);
            // Debug.Log("j : " + j);
            // Debug.Log("Triangle1 : " + (i * n + j ) + " (" + (i * n + j + i) + ", " + (i * n + j + i + 1) + ", " + (i * n + j + i + n + 2) + ")");
            // Debug.Log("Triangle2 : " + (i * n + j) + " (" + (i * n + j + i) + ", " + (i * n + j + i + n + 2) + ", " + (i * n + j + i + n + 1) + ")");

            triangl.Add((i + 2 * nbEtages - 1) % (nbEtages * nbMeridiens));
            triangl.Add(nbEtages * nbMeridiens + 1);
            triangl.Add(i + nbEtages - 1);

            for (int k = 0; k < nbEtages-1; k++)
            {
                triangl.Add((i + k + 1 + nbEtages) % (nbEtages * nbMeridiens));
                triangl.Add(i+k+1);
                triangl.Add(i + k);

                triangl.Add((i + k + nbEtages) % (nbEtages * nbMeridiens));
                triangl.Add((i+k+1+nbEtages) % (nbEtages * nbMeridiens));
                triangl.Add(i + k);
            }

            triangl.Add((i + nbEtages) % (nbEtages * nbMeridiens));
            triangl.Add(nbEtages * nbMeridiens);
            triangl.Add(i);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan : MonoBehaviour
{
    public Material mat;

    [Range(1, 100)]
    public int n = 2;
    [Range(1, 100)]
    public int offset = 10;

    void saveMeshInFile(Mesh msh, string file_path)
    {
        List<Vector3> listVertices = new List<Vector3>(msh.vertices);
        List<int> listTriangles = new List<int>(msh.triangles);

        string[] lines = new string[2 + (listVertices.Count) + (listTriangles.Count) / 3];

        lines[0] = "OFF";
        lines[1] = listVertices.Count + " " + (listTriangles.Count) / 3 + " " + (listTriangles.Count);

        for (int i = 0; i < listVertices.Count; i++)
        {
            lines[i + 2] = msh.vertices[i].x + " " + msh.vertices[i].y + " " + msh.vertices[i].z;
            lines[i + 2] = lines[i + 2].Replace(',', '.');
        }

        for (int j = 0, t = 0; j < (listTriangles.Count) / 3; j++)
        {
            lines[j + 2 + listVertices.Count] = "3" + " " + msh.triangles[t++] + " " + msh.triangles[t++] + " " + msh.triangles[t++];
        }

        System.IO.File.WriteAllLines(file_path, lines);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[(n + 1) * (n + 1)];
        int[] triangles = new int[6 * n * n];
        List<int> triangl = new List<int>();

        for (int i = 0; i < n + 1; i++)
        {
            for (int j = 0; j < n + 1; j++)
            {
                // Debug.Log("Vertices : " + (i * (n+1) + j) + " ("+ (offset * i) + ", " + (offset * j) +")");
                vertices[i * (n + 1) + j] = new Vector3(offset * i, offset * j, 0);
            }
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                // Debug.Log("i : " + i);
                // Debug.Log("j : " + j);
                // Debug.Log("Triangle1 : " + (i * n + j ) + " (" + (i * n + j + i) + ", " + (i * n + j + i + 1) + ", " + (i * n + j + i + n + 2) + ")");
                // Debug.Log("Triangle2 : " + (i * n + j) + " (" + (i * n + j + i) + ", " + (i * n + j + i + n + 2) + ", " + (i * n + j + i + n + 1) + ")");

                triangl.Add(i * n + j + i);
                triangl.Add(i * n + j + i + 1);
                triangl.Add(i * n + j + i + n + 2);

                triangl.Add(i * n + j + i);
                triangl.Add(i * n + j + i + n + 2);
                triangl.Add(i * n + j + i + n + 1);
            }
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

        saveMeshInFile(msh, "Assets/Maillages/plan.off");
    }

    void Update()
    {
        
    }
}

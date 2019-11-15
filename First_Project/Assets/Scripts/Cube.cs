using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[8];
        int[] triangles = new int[36];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(0, 1, 0);
        vertices[4] = new Vector3(0, 1, 1);
        vertices[5] = new Vector3(1, 1, 1);
        vertices[6] = new Vector3(1, 0, 1);
        vertices[7] = new Vector3(0, 0, 1);

        // Front

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 1;

        // Left

        triangles[6] = 7;
        triangles[7] = 4;
        triangles[8] = 3;

        triangles[9] = 7;
        triangles[10] = 3;
        triangles[11] = 0;

        // Right

        triangles[12] = 1;
        triangles[13] = 2;
        triangles[14] = 5;

        triangles[15] = 1;
        triangles[16] = 5;
        triangles[17] = 6;

        // Down

        triangles[18] = 0;
        triangles[19] = 6;
        triangles[20] = 7;

        triangles[21] = 0;
        triangles[22] = 1;
        triangles[23] = 6;

        // Up

        triangles[24] = 3;
        triangles[25] = 4;
        triangles[26] = 5;

        triangles[27] = 3;
        triangles[28] = 5;
        triangles[29] = 2;

        // Back

        triangles[30] = 6;
        triangles[31] = 5;
        triangles[32] = 4;

        triangles[33] = 6;
        triangles[34] = 4;
        triangles[35] = 7;


        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;

        saveMeshInFile(msh, "Assets/Maillages/cube.off");
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

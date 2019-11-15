using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class HandleTextFile : MonoBehaviour
{
    public bool active;
    public Material mat;
    void saveMeshInFile(Mesh msh, string file_path)
    {
        List<Vector3> listVertices = new List<Vector3>(msh.vertices);
        List<int> listTriangles = new List<int>(msh.triangles);

        string[] lines = new string[2 + (listVertices.Count) + (listTriangles.Count) /3];

        lines[0] = "OFF";
        lines[1] = listVertices.Count + " " + (listTriangles.Count) /3 + " " + (listTriangles.Count);

        for(int i = 0; i < listVertices.Count; i++)
        {
            lines[i + 2] = msh.vertices[i].x + " " + msh.vertices[i].y + " " + msh.vertices[i].z;
            lines[i + 2] = lines[i + 2].Replace(',', '.');
        }

        for(int j = 0, t = 0; j < (listTriangles.Count)/3; j++)
        {
            lines[j + 2 + listVertices.Count] = "3" + " " + msh.triangles[t++] + " " + msh.triangles[t++] + " " + msh.triangles[t++];
        }

        System.IO.File.WriteAllLines(file_path, lines);
    }

    public void readTextFileAndCreateMesh(string file_path)
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        string[] reader = System.IO.File.ReadAllLines(file_path);
        string[] nombres = reader[1].Split(' ');

        int nbSommets = int.Parse(nombres[0]);
        int nbTriangles = int.Parse(nombres[1]);
        int nbArretes = nbTriangles * 3;

        Vector3[] vertices = new Vector3[nbSommets];
        int[] triangles = new int[nbArretes];

        for (int i = 0; i < nbSommets; i++)
        {
            string[] line = reader[i + 2].Split(' ');

            float x = float.Parse(line[0].Replace('.', ','));
            float y = float.Parse(line[1].Replace('.', ','));
            float z = float.Parse(line[2].Replace('.', ','));

            vertices[i] = new Vector3(x, y, z);
        }

        for(int j = 0, t = 0; j < nbTriangles; j++)
        {
            string[] line = reader[j + 2 + nbSommets].Split(' ');
            triangles[t++] = int.Parse(line[1]);
            triangles[t++] = int.Parse(line[2]);
            triangles[t++] = int.Parse(line[3]);
        }

        Mesh msh = new Mesh();

        // Centrage
        Vector3 cdG = new Vector3(0, 0, 0);
        for(int i = 0; i < nbSommets; i++)
        {
            cdG += vertices[i];
        }
        cdG = cdG / nbSommets;
        gameObject.transform.position = cdG;

        // Normalisation
        float normMax = 0;
        for(int i = 0; i < nbSommets; i++)
        {
            if(normMax < vertices[i].magnitude) normMax = vertices[i].magnitude;
        }
        for (int i = 0; i < nbSommets; i++) vertices[i] = vertices[i] / normMax;

        // On ajoute les normales
        
        Vector3[] normTri = new Vector3[nbTriangles];
        Vector3[] normals = new Vector3[nbSommets];
        for (int j = 0, t = 0; j < nbTriangles; j++)
        {
            Vector3 pt1 = vertices[triangles[t++]];
            Vector3 pt2 = vertices[triangles[t++]];
            Vector3 pt3 = vertices[triangles[t++]];
            Vector3 vec1 = pt1 - pt2;
            Vector3 vec2 = pt2 - pt3;

            normTri[j] = Vector3.Cross(vec1, vec2).normalized;
        }

        for(int i = 0; i < nbSommets; i++)
        {
            int countSum = 0;
            Vector3 normSum = new Vector3(0, 0, 0);
            for(int j = 0, t = 0; j < nbTriangles; j++)
            {
                if (triangles[t++] == i)
                {
                    normSum += normTri[j];
                    countSum++;
                }
                else if (triangles[t++] == i)
                {
                    normSum += normTri[j];
                    countSum++;
                }
                else if (triangles[t++] == i)
                {
                    normSum += normTri[j];
                    countSum++;
                }
            }
            normals[i] = (normSum / countSum);
            normals[i] = normals[i].normalized;
        }

        msh.vertices = vertices;
        msh.triangles = triangles;
        msh.normals = normals;
        
        // logMeshProperties(msh);
        // saveMeshInFile(msh, "Assets/Maillages/test.off");

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    void logMeshProperties(Mesh msh)
    {
        List<Vector3> listVertices = new List<Vector3>(msh.vertices);
        List<int> listTriangles = new List<int>(msh.triangles);
        List<Vector2> listArretes = new List<Vector2>();

        for (int j = 0, t = 0; j < listTriangles.Count/3; j++)
        {
            int pt1 = listTriangles[t++];
            int pt2 = listTriangles[t++];
            int pt3 = listTriangles[t++];

            Vector2 vec1;
            Vector2 vec2;
            Vector2 vec3;

            if (pt1 < pt2)
            {
                vec1 = new Vector2(pt1, pt2);
            }
            else
            {
                vec1 = new Vector2(pt2, pt1);
            }

            if (pt2 < pt3)
            {
                vec2 = new Vector2(pt2, pt3);
            }
            else
            {
                vec2 = new Vector2(pt3, pt2);
            }

            if (pt1 < pt3)
            {
                vec3 = new Vector2(pt1, pt3);
            }
            else
            {
                vec3 = new Vector2(pt3, pt1);
            }

            if (!listArretes.Contains(vec1)) listArretes.Add(vec1);
            if (!listArretes.Contains(vec2)) listArretes.Add(vec2);
            if (!listArretes.Contains(vec3)) listArretes.Add(vec3);
        }

        Debug.Log("Nombre de Sommets : " + listVertices.Count);
        Debug.Log("Nombre de Faces : " + (listTriangles.Count / 3));
        Debug.Log("Nombre d'Arêtes : " + listArretes.Count);

        int maxArretes = 0;
        int sommetMax = -1;
        int minArretes = 1000;
        int sommetMin = -1;
        for(int i = 0; i < listVertices.Count; i++)
        {
            int compteSommetI = 0;

            for (int j = 0; j < listVertices.Count; j++)
            {
                Vector2 v1 = new Vector2(i, j);
                Vector2 v2 = new Vector2(j, i);

                if (listArretes.Contains(v1) || listArretes.Contains(v2))
                {
                    compteSommetI++;
                }
            }

            if(compteSommetI < minArretes)
            {
                minArretes = compteSommetI;
                sommetMin = i;
            }

            if(compteSommetI > maxArretes)
            {
                maxArretes = compteSommetI;
                sommetMax = i;
            }
        }

        Debug.Log("Sommet n°" + sommetMax + " possede le max d'arretes etant de : " + maxArretes);
        Debug.Log("Sommet n°" + sommetMin + " possede le min d'arretes etant de : " + minArretes);

        int[] countArretes = new int[listVertices.Count * listVertices.Count];
        for(int i = 0; i < listVertices.Count; i++)
        {
            for(int j = 0; j < listVertices.Count; j++)
            {
                countArretes[i + j*listVertices.Count] = -1;
            }
        }

        for (int j = 0, t = 0; j < listTriangles.Count / 3; j++)
        {
            int pt1 = listTriangles[t++];
            int pt2 = listTriangles[t++];
            int pt3 = listTriangles[t++];

            Vector2 vec1;
            Vector2 vec2;
            Vector2 vec3;

            if (pt1 < pt2)
            {
                vec1 = new Vector2(pt1, pt2);
            }
            else
            {
                vec1 = new Vector2(pt2, pt1);
            }

            if (pt2 < pt3)
            {
                vec2 = new Vector2(pt2, pt3);
            }
            else
            {
                vec2 = new Vector2(pt3, pt2);
            }

            if (pt1 < pt3)
            {
                vec3 = new Vector2(pt1, pt3);
            }
            else
            {
                vec3 = new Vector2(pt3, pt1);
            }

            if (listArretes.Contains(vec1)) countArretes[(int)vec1.x + (int)vec1.y*listVertices.Count] += 1;
            if (listArretes.Contains(vec2)) countArretes[(int)vec2.x + (int)vec2.y*listVertices.Count] += 1;
            if (listArretes.Contains(vec3)) countArretes[(int)vec3.x + (int)vec3.y*listVertices.Count] += 1;
        }

        for (int i = 0; i < listVertices.Count; i++)
        {
            for (int j = 0; j < listVertices.Count; j++)
            {
                if (countArretes[i + j * listVertices.Count] < 2 && countArretes[i + j * listVertices.Count] != -1) Debug.Log("L'arête utilisant les sommets " + i + " et " + j + " est partagee par 0 ou 1 face.");
            }
        }

        /*
        Debug.Log("Nombre d'Arêtes par Face : " + 3);

        // Etablir une liste des Arêtes
        int countTotal = 0;
        for(int i = 0; i < listVertices.Count; i++)
        {
            int countI = 0;
            for(int j = 0; j < listTriangles.Count; j++)
            {
                if (listTriangles[j] == i) countI++;
            }
            if (countI < 2) countTotal++;
        }

        Debug.Log("Nombre d'Arêtes partagées par 0 ou 1 Face : " + countTotal);
        */
    }

    void Start()
    {
        if(active) readTextFileAndCreateMesh("Assets/Maillages/bunny.off");
    }

}

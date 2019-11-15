using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSimplify : MonoBehaviour
{
    public Material mat;
    [Range(1, 50)]
    public int tailleGrille = 10;

    private bool inf(Vector3 a, Vector3 b)
    {
        return a.x <= b.x && a.y <= b.y && a.z <= b.z;
    }

    private bool sup(Vector3 a, Vector3 b)
    {
        return a.x > b.x && a.y > b.y && a.z > b.z;
    }

    void simplification()
    {
        GetComponent<HandleTextFile>().readTextFileAndCreateMesh("Assets/Maillages/bunny.off");
        Mesh newMESH = new Mesh();

        List<Vector3> listVertices = new List<Vector3>(GetComponent<MeshFilter>().mesh.vertices);
        List<int> listTriangles = new List<int>(GetComponent<MeshFilter>().mesh.triangles);

        Vector3 parcours = new Vector3(1f / (float)tailleGrille, 1f / (float)tailleGrille, 1f / (float)tailleGrille);

        //Parcours de la grille
        for (int i = -tailleGrille; i < tailleGrille; i++)
        {
            float x = i * parcours.x;
            for (int j = -tailleGrille; j < tailleGrille; j++)
            {
                float y = j * parcours.y;
                for (int k = -tailleGrille; k < tailleGrille; k++)
                {
                    float z = k * parcours.z;

                    //Limite de la grille
                    Vector3 min = new Vector3(x, y, z);
                    Vector3 max = min + parcours;

                    List<int> listGrilleInt = new List<int>();//on gardera un seul point de cette grille

                    //Parcours des sommets
                    for (int v = 0; v < listVertices.Count; v++)
                    {
                        if (sup(listVertices[v], min) && inf(listVertices[v], max))
                        {
                            //le point est dans notre grille actuelle
                            //il faut garder qu il seul point
                            listGrilleInt.Add(v);
                        }
                    }

                    //on n en garde qu'un seul --> le premier
                    if (listGrilleInt.Count > 0)
                    {
                        int index = listGrilleInt[0];

                        //parcours des triangles
                        for (int t = 0; t < listTriangles.Count; t++)
                        {
                            if (listGrilleInt.Contains(listTriangles[t]))
                            {
                                listTriangles[t] = index;
                            }
                        }
                    }
                }
            }
        }

        //remove useless triangles
        for (int i = 0; i < listTriangles.Count; i += 3)
        {
            if (listTriangles[i] == listTriangles[i + 1] && listTriangles[i] == listTriangles[i + 2])
            {
                //remove triangles
                listTriangles.RemoveAt(i);
                listTriangles.RemoveAt(i);
                listTriangles.RemoveAt(i);
                i -= 3;
            }
        }

        Vector3[] vertices = new Vector3[listVertices.Count];
        int[] triangles = new int[listTriangles.Count];

        newMESH.vertices = listVertices.ToArray();
        newMESH.triangles = listTriangles.ToArray();

        GetComponent<MeshFilter>().mesh = newMESH;
        GetComponent<MeshFilter>().mesh.RecalculateNormals();
        GetComponent<MeshRenderer>().material = mat;
    }

    // Start is called before the first frame update
    void Start()
    {
        simplification();
        transform.position = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            simplification();
            transform.position = new Vector3(1, 0, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumicSphere : MonoBehaviour
{

    public GameObject cube;
    public float potentiel = 0;
    public bool intersection = false;
    public bool union = true;
    public bool melange = false;
    public int tailleCube = 1;

    [System.Serializable]
    public class Sphere
    {
        public Vector3 centre;
        public float rayon;

        public Sphere(Vector3 center, float ray)
        {
            centre = center;
            rayon = ray;
        }
    }

    [System.Serializable]
    public class Quadrique
    {
        public Vector3 centre;
        public float a;
        public float b;
        public float c;
        public bool ellipsoide;
        public bool hyperbole1Nappe;
        public bool paraHyper;

        public Quadrique(Vector3 center, float _a, float _b, float _c, bool _ellispoide, bool _hyperbole1Nappe, bool _paraHyper)
        {
            a = _a;
            b = _b;
            c = _c;

            ellipsoide = _ellispoide;
            hyperbole1Nappe = _hyperbole1Nappe;
            paraHyper = _paraHyper;

            centre = center;
        }
    }

    bool isEqQuadriqueEllipsoide(Vector3 point, Quadrique q)
    {
        if ((((point.x - q.centre.x) * (point.x - q.centre.x))/(q.a*q.a) + ((point.y - q.centre.y) * (point.y - q.centre.y)) / (q.b * q.b) + ((point.z - q.centre.z) * (point.z - q.centre.z)) / (q.c * q.c) - 1) >= potentiel) return false;
        return true;
    }

    bool isEqQuadriqueHyperbole1Nappe(Vector3 point, Quadrique q)
    {
        if ((((point.x - q.centre.x) * (point.x - q.centre.x)) / (q.a * q.a) + ((point.y - q.centre.y) * (point.y - q.centre.y)) / (q.b * q.b) - ((point.z - q.centre.z) * (point.z - q.centre.z)) / (q.c * q.c) - 1) >= potentiel) return false;
        return true;
    }

    bool isEqQuadriqueParaHyper(Vector3 point, Quadrique q)
    {
        if ((((point.x - q.centre.x) * (point.x - q.centre.x)) / (q.a * q.a) - ((point.y - q.centre.y) * (point.y - q.centre.y)) / (q.b * q.b)) - (point.z - q.centre.z) != potentiel) return false;
        return true;
    }

    bool isEqSphere(Vector3 point, Sphere s)
    {
        if (((point.x -s.centre.x)* (point.x - s.centre.x) + (point.y - s.centre.y) * (point.y - s.centre.y)+ (point.z - s.centre.z) * (point.z - s.centre.z) - (s.rayon) * (s.rayon)) >= potentiel) return false;
        return true;
    }

    public List<Sphere> listSphere;
    public List<Quadrique> listQuadrique;

    // Start is called before the first frame update
    void Start()
    {
        cube.transform.localScale = new Vector3(tailleCube, tailleCube, tailleCube);

        Vector3 min;
        Vector3 max;

        min = listSphere[0].centre - new Vector3(listSphere[0].rayon, listSphere[0].rayon, listSphere[0].rayon);
        max = listSphere[0].centre + new Vector3(listSphere[0].rayon, listSphere[0].rayon, listSphere[0].rayon);

        for (int i = 0; i < listSphere.Count; i++)
        {
            if (listSphere[i].centre.x - listSphere[i].rayon < min.x) min.x = listSphere[i].centre.x - listSphere[i].rayon;
            if (listSphere[i].centre.y - listSphere[i].rayon < min.y) min.y = listSphere[i].centre.y - listSphere[i].rayon;
            if (listSphere[i].centre.z - listSphere[i].rayon < min.z) min.z = listSphere[i].centre.z - listSphere[i].rayon;

            if (listSphere[i].centre.x + listSphere[i].rayon > max.x) max.x = listSphere[i].centre.x + listSphere[i].rayon;
            if (listSphere[i].centre.y + listSphere[i].rayon > max.y) max.y = listSphere[i].centre.y + listSphere[i].rayon;
            if (listSphere[i].centre.z + listSphere[i].rayon > max.z) max.z = listSphere[i].centre.z + listSphere[i].rayon;
        }

        Vector3 boundingBox = max - min;

        for(int x = (int)-boundingBox.x; x < (int)+boundingBox.x; x += tailleCube)
        {
            for (int y = (int)-boundingBox.y; y < (int)+boundingBox.y; y += tailleCube)
            {
                for (int z = (int)-boundingBox.z; z < (int)+boundingBox.z; z += tailleCube)
                {
                    bool isIntersection = true;
                    // Intersection
                    if (intersection) 
                    {
                        for(int i = 0; i < listSphere.Count; i++)
                        {
                            isIntersection &= isEqSphere(new Vector3(x, y, z), listSphere[i]); 
                        }
                        for (int i = 0; i < listQuadrique.Count; i++)
                        {
                            if (listQuadrique[i].ellipsoide) isIntersection &= isEqQuadriqueEllipsoide(new Vector3(x, y, z), listQuadrique[i]);
                            if (listQuadrique[i].hyperbole1Nappe) isIntersection &= isEqQuadriqueHyperbole1Nappe(new Vector3(x, y, z), listQuadrique[i]);
                            if (listQuadrique[i].paraHyper) isIntersection &= isEqQuadriqueParaHyper(new Vector3(x, y, z), listQuadrique[i]);
                        }
                        if (isIntersection)
                        {
                            Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                        }
                        
                    }
                    // Union
                    if (union)
                    {
                        for (int i = 0; i < listSphere.Count; i++)
                        {
                            if(isEqSphere(new Vector3(x, y, z), listSphere[i]))
                            {
                                Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                            }
                        }
                        for (int i = 0; i < listQuadrique.Count; i++)
                        {
                            if (listQuadrique[i].ellipsoide)
                            {
                                if (isEqQuadriqueEllipsoide(new Vector3(x, y, z), listQuadrique[i]))
                                {
                                    Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                                }
                            }

                            if (listQuadrique[i].hyperbole1Nappe)
                            {
                                if (isEqQuadriqueHyperbole1Nappe(new Vector3(x, y, z), listQuadrique[i]))
                                {
                                    Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                                }
                            }

                            if (listQuadrique[i].paraHyper)
                            {
                                if (isEqQuadriqueParaHyper(new Vector3(x, y, z), listQuadrique[i]))
                                {
                                    Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                                }
                            }
                        }
                    }

                    if (melange)
                    {
                       
                    }
                }
            }
        }
    }
}
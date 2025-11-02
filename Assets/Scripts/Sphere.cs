using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereGenerator : MonoBehaviour
{
    [Header("Paramètres de la sphère")]
    public float rayon = 1f;
    public int nbParalleles = 12; //nmbr anneaux
    public int nbMeridiens = 24;  

    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreerSphere(rayon, nbParalleles, nbMeridiens);
    }

    Mesh CreerSphere(float rayon, int nbParalleles, int nbMeridiens)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Sphère";

        
        Vector3[] vertices = new Vector3[2 + (nbParalleles - 1) * nbMeridiens]; //haut et bas de la sphère et anneaux entre les deux
        vertices[0] = new Vector3(0, rayon, 0); 
        vertices[vertices.Length - 1] = new Vector3(0, -rayon, 0); 

        int index = 1;
        for (int i = 1; i < nbParalleles; i++)
        {
            float phi = Mathf.PI * i / nbParalleles;
            float z = rayon * Mathf.Cos(phi);
            float r = rayon * Mathf.Sin(phi);

            for (int j = 0; j < nbMeridiens; j++)
            {
                float theta = 2 * Mathf.PI * j / nbMeridiens;
                float x = r * Mathf.Cos(theta);
                float y = r * Mathf.Sin(theta);
                vertices[index++] = new Vector3(x, z, y);
            }
        }

        
        int nbTriangles = 6 * (nbParalleles - 1) * nbMeridiens;
        int[] triangles = new int[nbTriangles];
        int t = 0;

        
        for (int j = 0; j < nbMeridiens; j++)
        {
            int next = (j + 1) % nbMeridiens;
            triangles[t++] = 0;
            triangles[t++] = 1 + next;
            triangles[t++] = 1 + j;
        }

       
        for (int i = 0; i < nbParalleles - 2; i++)
        {
            for (int j = 0; j < nbMeridiens; j++)
            {
                int next = (j + 1) % nbMeridiens;
                int a = 1 + i * nbMeridiens + j;
                int b = 1 + i * nbMeridiens + next;
                int c = 1 + (i + 1) * nbMeridiens + next;
                int d = 1 + (i + 1) * nbMeridiens + j;

                triangles[t++] = a; triangles[t++] = b; triangles[t++] = c;
                triangles[t++] = a; triangles[t++] = c; triangles[t++] = d;
            }
        }

        
        int lastParallelStart = 1 + (nbParalleles - 2) * nbMeridiens;
        int southPole = vertices.Length - 1;

        for (int j = 0; j < nbMeridiens; j++)
        {
            int next = (j + 1) % nbMeridiens;
            triangles[t++] = lastParallelStart + j;
            triangles[t++] = southPole;
            triangles[t++] = lastParallelStart + next;
        }

        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}

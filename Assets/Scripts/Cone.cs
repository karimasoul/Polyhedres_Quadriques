using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeGenerator : MonoBehaviour
{
    [Header("Paramètres du cône")]
    public float rayonBase = 1f;
    public float rayonTop = 0f; 
    public float hauteur = 2f;
    public int nbMeridiens = 24;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreerCone(rayonBase, rayonTop, hauteur, nbMeridiens);
    }

    Mesh CreerCone(float rayonBase, float rayonTop, float hauteur, int nbMeridiens)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Cone";

        
        Vector3[] vertices = new Vector3[(nbMeridiens + 1) * 2];
        float demiHauteur = hauteur / 2f;

        for (int i = 0; i <= nbMeridiens; i++)
        {
            float angle = 2 * Mathf.PI * i / nbMeridiens;
            float xBase = rayonBase * Mathf.Cos(angle);
            float zBase = rayonBase * Mathf.Sin(angle);
            float xTop = rayonTop * Mathf.Cos(angle);
            float zTop = rayonTop * Mathf.Sin(angle);

            vertices[i] = new Vector3(xBase, -demiHauteur, zBase); //Cercle du bas
            vertices[i + nbMeridiens + 1] = new Vector3(xTop, demiHauteur, zTop); //Cercle du haut
        }

        
        int nbTriangles = nbMeridiens * 12; //faces + 2 disques
        int[] triangles = new int[nbTriangles];
        int t = 0;

        //Faces
        for (int i = 0; i < nbMeridiens; i++)
        {
            int next = (i + 1) % (nbMeridiens + 1);

            int a = i;
            int b = next;
            int c = i + nbMeridiens + 1;
            int d = next + nbMeridiens + 1;

            triangles[t++] = a; triangles[t++] = c; triangles[t++] = b;
            triangles[t++] = b; triangles[t++] = c; triangles[t++] = d;
        }

       
        Vector3 centreBase = new Vector3(0, -demiHauteur, 0);
        int centreBaseIndex = vertices.Length;
        System.Array.Resize(ref vertices, vertices.Length + 1);
        vertices[centreBaseIndex] = centreBase;

        for (int i = 0; i < nbMeridiens; i++)
        {
            int next = (i + 1) % (nbMeridiens + 1);
            triangles[t++] = centreBaseIndex;
            triangles[t++] = next;
            triangles[t++] = i;
        }

        
        if (rayonTop > 0)
        {
            Vector3 centreTop = new Vector3(0, demiHauteur, 0);
            int centreTopIndex = vertices.Length;
            System.Array.Resize(ref vertices, vertices.Length + 1);
            vertices[centreTopIndex] = centreTop;

            for (int i = 0; i < nbMeridiens; i++)
            {
                int next = (i + 1) % (nbMeridiens + 1);
                triangles[t++] = centreTopIndex;
                triangles[t++] = i + nbMeridiens + 1;
                triangles[t++] = next + nbMeridiens + 1;
            }
        }

        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}

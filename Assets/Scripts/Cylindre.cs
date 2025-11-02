using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CylindreGenerator : MonoBehaviour
{
    [Header("Paramètres du cylindre")]
    public float rayon = 1f;
    public float hauteur = 2f;
    [Range(3, 100)]
    public int nbMeridiens = 20;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreerCylindre(rayon, hauteur, nbMeridiens);
    }

    Mesh CreerCylindre(float r, float h, int m)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Cylindre";

        
        Vector3[] sommets = new Vector3[m * 2 + 2]; //2 par méridien + 2 centres
        int idx = 0;

        for (int i = 0; i < m; i++)
        {
            float angle = 2 * Mathf.PI * i / m;
            float x = r * Mathf.Cos(angle);
            float y = r * Mathf.Sin(angle);

            // Bas
            sommets[idx++] = new Vector3(x, y, -h / 2);
            // Haut
            sommets[idx++] = new Vector3(x, y, h / 2);
        }

        
        Vector3 centreBas = new Vector3(0, 0, -h / 2);
        Vector3 centreHaut = new Vector3(0, 0, h / 2);
        sommets[idx++] = centreBas;
        sommets[idx++] = centreHaut;

        
        int nbTriangles = m * 4; 
        int[] triangles = new int[nbTriangles * 3];
        int t = 0;

        int indexCentreBas = m * 2;
        int indexCentreHaut = m * 2 + 1;

        for (int i = 0; i < m; i++)
        {
            int iBas = i * 2;
            int iHaut = i * 2 + 1;
            int iBasNext = (i * 2 + 2) % (m * 2);
            int iHautNext = (i * 2 + 3) % (m * 2);

           
            //triangle 1
            triangles[t++] = iBas;
            triangles[t++] = iHaut;
            triangles[t++] = iHautNext;

            //triangle 2
            triangles[t++] = iBas;
            triangles[t++] = iHautNext;
            triangles[t++] = iBasNext;

            //bas
            triangles[t++] = indexCentreBas;
            triangles[t++] = iBasNext;
            triangles[t++] = iBas;

            //haut
            triangles[t++] = indexCentreHaut;
            triangles[t++] = iHaut;
            triangles[t++] = iHautNext;
        }

        
        Vector2[] uvs = new Vector2[sommets.Length];
        for (int i = 0; i < sommets.Length; i++)
            uvs[i] = new Vector2(sommets[i].x, sommets[i].y);

        
        mesh.vertices = sommets;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}

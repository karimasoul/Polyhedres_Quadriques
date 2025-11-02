using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CylindreTronqueGenerator : MonoBehaviour
{
    [Header("Paramètres du cylindre tronqué")]
    public float rayon = 1f;
    public float hauteur = 2f;
    [Range(3, 100)]
    public int nbMeridiens = 20;

    [Header("Tronquage")]
    [Range(0, 360)]
    public float angleMin = 0f;   
    [Range(0, 360)]
    public float angleMax = 270f;    

    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreerCylindreTronque(rayon, hauteur, nbMeridiens, angleMin, angleMax);
    }

    Mesh CreerCylindreTronque(float r, float h, int m, float aMin, float aMax)
    {
        Mesh mesh = new Mesh();
        mesh.name = "CylindreTronque";

        int nbPoints = m + 1; 
        Vector3[] sommets = new Vector3[nbPoints * 2 + 2]; 
        int idx = 0;

        for (int i = 0; i < nbPoints; i++)
        {
            float angle = Mathf.Deg2Rad * Mathf.Lerp(aMin, aMax, (float)i / m);
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

        int[] triangles = new int[m * 4 * 3];
        int t = 0;

        int indexCentreBas = nbPoints * 2;
        int indexCentreHaut = nbPoints * 2 + 1;

        for (int i = 0; i < m; i++)
        {
            int iBas = i * 2;
            int iHaut = i * 2 + 1;
            int iBasNext = (i + 1) * 2;
            int iHautNext = (i + 1) * 2 + 1;

            
            triangles[t++] = iBas;
            triangles[t++] = iHaut;
            triangles[t++] = iHautNext;

            triangles[t++] = iBas;
            triangles[t++] = iHautNext;
            triangles[t++] = iBasNext;

           
            triangles[t++] = indexCentreBas;
            triangles[t++] = iBasNext;
            triangles[t++] = iBas;

            
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

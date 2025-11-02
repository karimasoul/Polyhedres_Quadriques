using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereTronqueeGenerator : MonoBehaviour
{
    [Header("Paramètres de la sphère")]
    public float rayon = 1f;
    [Range(3, 100)]
    public int nbMeridiens = 20;   
    [Range(2, 100)]
    public int nbParalleles = 20;  

    [Header("Tronquage")]
    [Range(0, 360)]
    public float angleMin = 0f;    
    [Range(0, 360)]
    public float angleMax = 270f; 
    [Range(-90, 90)]
    public float latitudeMin = -45f; 
    [Range(-90, 90)]
    public float latitudeMax = 45f;  
    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreerSphereTronquee(rayon, nbMeridiens, nbParalleles, angleMin, angleMax, latitudeMin, latitudeMax);
    }

    Mesh CreerSphereTronquee(float r, int m, int p, float aMin, float aMax, float latMin, float latMax)
    {
        Mesh mesh = new Mesh();
        mesh.name = "SphereTronquee";

        int nbPointsMeridiens = m + 1;
        int nbPointsParalleles = p + 1;
        Vector3[] sommets = new Vector3[nbPointsMeridiens * nbPointsParalleles];
        int idx = 0;

        for (int j = 0; j < nbPointsParalleles; j++)
        {
            float v = (float)j / p;
            float latitude = Mathf.Lerp(latMin, latMax, v) * Mathf.Deg2Rad;
            float y = r * Mathf.Sin(latitude);
            float rayonCercle = r * Mathf.Cos(latitude);

            for (int i = 0; i < nbPointsMeridiens; i++)
            {
                float u = (float)i / m;
                float angle = Mathf.Lerp(aMin, aMax, u) * Mathf.Deg2Rad;
                float x = rayonCercle * Mathf.Cos(angle);
                float z = rayonCercle * Mathf.Sin(angle);
                sommets[idx++] = new Vector3(x, y, z);
            }
        }

        
        int[] triangles = new int[m * (p - 1) * 6];
        int t = 0;

        for (int j = 0; j < p; j++)
        {
            for (int i = 0; i < m; i++)
            {
                int current = j * nbPointsMeridiens + i;
                int next = current + nbPointsMeridiens;

                if (j < p - 1)
                {
                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;

                    triangles[t++] = current;
                    triangles[t++] = next + 1;
                    triangles[t++] = current + 1;
                }
            }
        }

        Vector2[] uvs = new Vector2[sommets.Length];
        for (int i = 0; i < sommets.Length; i++)
            uvs[i] = new Vector2((sommets[i].x / r + 1f) / 2f, (sommets[i].y / r + 1f) / 2f);

        mesh.vertices = sommets;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}

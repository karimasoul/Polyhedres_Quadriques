using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode] 
public class GridPlaneGenerator : MonoBehaviour
{
    [Range(1, 500)]
    public int nbLignes = 10;    
    [Range(1, 500)]
    public int nbColonnes = 10;  
    public float width = 10f;    
    public float height = 10f;   

    void Start()
    {
        Generate();
    }

    
    void OnValidate()
    {
        
        if (Application.isPlaying) return;
        Generate();
    }

    public void Generate()
    {
        Mesh mesh = new Mesh();
        mesh.name = "GridPlane";

        int vertCountX = nbColonnes + 1; //nmbr point sur x
        int vertCountY = nbLignes + 1; // nmbr point sur y
        int numVertices = vertCountX * vertCountY; //nmbr total de sommet
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        //distances entre les points
        float stepX = width / nbColonnes;
        float stepY = height / nbLignes;

        int idx = 0;
        for (int y = 0; y < vertCountY; y++)
        {
            for (int x = 0; x < vertCountX; x++)
            {
                float px = x * stepX; //position point hori
                float py = y * stepY; //position point vert
                vertices[idx] = new Vector3(px, py, 0f);
                uv[idx] = new Vector2((float)x / nbColonnes, (float)y / nbLignes);
                idx++;
            }
        }

        
        int numCells = nbColonnes * nbLignes;
        int[] triangles = new int[numCells * 6]; //un triangle fait 3 donc 2 triangle fait 6
        int t = 0;
        for (int y = 0; y < nbLignes; y++)
        {
            for (int x = 0; x < nbColonnes; x++)
            {
                int v00 = y * vertCountX + x;         // bas gauche
                int v10 = v00 + 1;                    // bas droit
                int v01 = v00 + vertCountX;           // haut gauche
                int v11 = v01 + 1;                    // haut droit

                //triangle 1 
                triangles[t++] = v00;
                triangles[t++] = v01;
                triangles[t++] = v10;

                //triangle 2
                triangles[t++] = v10;
                triangles[t++] = v01;
                triangles[t++] = v11;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}

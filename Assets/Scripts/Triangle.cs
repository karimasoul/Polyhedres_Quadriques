using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateTriangle : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Triangle";

        Vector3[] vertices = new Vector3[] //tableau des sommets
        {
            new Vector3(0f, 0f, 0f),   
            new Vector3(1f, 0f, 0f),   
            new Vector3(0f, 1f, 0f)    
        };

        int[] triangles = new int[] { 0, 2, 1 }; 

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1)
        };

        mesh.vertices = vertices; //les sommets
        mesh.triangles = triangles; 
        mesh.uv = uv;
        mesh.RecalculateNormals(); //fnc pour les normales

        GetComponent<MeshFilter>().mesh = mesh;
    }
}

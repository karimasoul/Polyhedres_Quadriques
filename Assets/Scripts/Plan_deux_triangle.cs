using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateQuad : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Quad";

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f), //bas gauche
            new Vector3(1f, 0f, 0f), //bas droit
            new Vector3(0f, 1f, 0f), //haut gauche
            new Vector3(1f, 1f, 0f)  //haut droit
        };

        
        int[] triangles = new int[]
        {
            0, 2, 1, //forme le 1er triangle
            2, 3, 1  //forme le 2eme
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}

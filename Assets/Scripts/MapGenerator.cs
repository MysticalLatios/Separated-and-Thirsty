using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Mesh plane;
    private MeshCollider myCollider;
    public float maxHight,qulity;

    // Start is called before the first frame update
    void Start()
    {
        plane = GetComponent<MeshFilter>().mesh;
        myCollider = GetComponent<MeshCollider>();
        generatePerlinHill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generatePerlinHill()
    {
        Vector3[] vertices = plane.vertices;

        for (int i = 0; i< vertices.Length; i++)
        {
            float px = transform.position.x + vertices[i].x / qulity;
            float pz = transform.position.z + vertices[i].z / qulity;

            vertices[i].y = Mathf.PerlinNoise(px, pz) * maxHight;

        }

        plane.vertices = vertices;
        plane.RecalculateBounds();
        plane.RecalculateNormals();
        myCollider.sharedMesh = plane;
    }
}

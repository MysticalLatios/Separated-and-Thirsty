using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapGenerator : NetworkBehaviour
{
    public Mesh plane;
    private MeshCollider myCollider;
    public float maxHight,quality;

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
        int seedX = Random.Range(0, 1000);
        int seedZ = Random.Range(0, 1000);

        for (int i = 0; i< vertices.Length; i++)
        {
            float px = (transform.position.x + vertices[i].x) / quality;
            float pz = (transform.position.z + vertices[i].z) / quality;

            vertices[i].y = Mathf.PerlinNoise(px + seedX, pz + seedZ) * maxHight - maxHight / 2;
            vertices[i].x *= 50;
            vertices[i].z *= 50;
        }

        plane.vertices = vertices;
        plane.RecalculateBounds();
        plane.RecalculateNormals();
        myCollider.sharedMesh = plane;
    }
}



// Start is called before the first frame update
//        void Start()
//        {
//            myCollider = GetComponent<MeshCollider>();
//
//            CombineInstance[] planes = new CombineInstance[SIZE * SIZE];
//
//            for (int x = 0; x < SIZE; x ++)
//            {
//                for (int z = 0; z < SIZE; z++)
//                {
//                    planes[x * SIZE + z].mesh = new MeshFilter().mesh;
//                    planes[x * SIZE + z].mesh = generatePerlinHill(planes[x * SIZE + z].mesh, x - SIZE / 2, z - SIZE / 2);
//                    planes[x * SIZE + z].transform = new Matrix4x4(new Vector4(), new Vector4(), new Vector4(), new Vector4(x - SIZE / 2, 0, z - SIZE / 2));
//                }
//            }
//
//            //myCollider.sharedMesh.Clear();
//            plane = GetComponent<MeshFilter>().mesh = new Mesh();
//            plane.CombineMeshes(planes);
//            plane.RecalculateBounds();
//            plane.RecalculateNormals();
//            myCollider.sharedMesh = plane;
//        }
//
//        // Update is called once per frame
//        void Update()
//        {
//       
//        }
//
//        Mesh generatePerlinHill(Mesh plane, int offX, int offZ)
//        {
//            Vector3[] vertices = plane.vertices;
//
//            for (int i = 0; i< vertices.Length; i++)
//            {
//                float px = (offX + vertices[i].x) / quality;
//                float pz = (offZ + vertices[i].z) / quality;
//
//                vertices[i].y = Mathf.PerlinNoise(px, pz) * maxHight;
//
//            }
//
//            plane.vertices = vertices;
//
//            return plane;
//        }
//}
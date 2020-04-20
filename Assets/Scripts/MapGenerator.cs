using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MapGenerator : NetworkBehaviour
{
    private MeshCollider myCollider;
    private MeshFilter filter;

    [SyncVar(hook = nameof(genMap))]
    public Vector2 seeds;

    private void genMap(UnityEngine.Vector2 oldValue, UnityEngine.Vector2 newValue)
    {
        seeds = newValue;
        Debug.Log("genmap running");
    }

    public float maxHight,quality,mapLength,mapWidth;


    void Awake()
    {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        filter = this.GetComponent<MeshFilter>();
        Debug.Log("added plane");
        this.myCollider = GetComponent<MeshCollider>();
        Debug.Log("seed x : " + seeds.x + ",  y : " + seeds.y);
        generatePerlinHill();
    }


    public override void OnStartServer()
    {
        randomizeSeed();
    } 

    public void initColiders()
    {
        Debug.Log("added plane");
        this.myCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void randomizeSeed()
    {
        seeds = new Vector2(UnityEngine.Random.Range(0, 1000), UnityEngine.Random.Range(0, 1000));
        
    }


    public void generatePerlinHill()
    {
        Vector3[] plane = filter.mesh.vertices;

        for (int i = 0; i< plane.Length; i++)
        {
            float px = (transform.position.x + plane[i].x) / quality;
            float pz = (transform.position.z + plane[i].y) / quality;

            float perlinHeight = Mathf.PerlinNoise(px + seeds.x, pz + seeds.y) * maxHight - maxHight / 2;
            Vector3 vec = new Vector3(plane[i].x * mapLength, plane[i].y * mapWidth, perlinHeight);
            plane[i] = vec;
        }

        filter.mesh.vertices = plane;
        filter.mesh.RecalculateNormals();
        filter.mesh.RecalculateBounds();
        Debug.Log("Replacing mesh");
        myCollider.sharedMesh = filter.mesh;
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
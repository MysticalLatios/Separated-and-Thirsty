using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MapGenerator : NetworkBehaviour
{
    private Mesh plane;
    private MeshCollider myCollider;

    [SyncVar(hook = nameof(genMap))]
    public Vector2 seeds;

    private void genMap(UnityEngine.Vector2 oldValue, UnityEngine.Vector2 newValue)
    {
        seeds = newValue;
        Debug.Log("genmap running");
    }

    public float maxHight,quality;


    void Awake()
    {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.plane = this.GetComponent<MeshFilter>().mesh;
        Debug.Log("added plane");
        this.myCollider = GetComponent<MeshCollider>();
        Debug.Log("seed x : " + mapGen.seeds.x + ",  y : " + mapGen.seeds.y);
        generatePerlinHill();
    }


    public override void OnStartServer()
    {
        randomizeSeed();
    } 

    public void initColiders()
    {
        this.plane = this.GetComponent<MeshFilter>().mesh;
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
        Vector3[] vertices = plane.vertices;
      

        for (int i = 0; i< vertices.Length; i++)
        {
            float px = (transform.position.x + vertices[i].x) / quality;
            float pz = (transform.position.z + vertices[i].z) / quality;

            vertices[i].y = Mathf.PerlinNoise(px + seeds.x, pz + seeds.y) * maxHight - maxHight / 2;
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
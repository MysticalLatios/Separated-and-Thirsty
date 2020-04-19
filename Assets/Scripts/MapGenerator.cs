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


    [SyncVar]
    public int SpawnCarOne;
    [SyncVar]
    public int SpawnCarTwo;

    public Transform CarSpawn;
    public Transform Car2Spawn;

    

    private void genMap(UnityEngine.Vector2 oldValue, UnityEngine.Vector2 newValue)
    {
        seeds = newValue;
        Debug.Log("genmap running");
    }

    public float maxHight,quality,xMod,yMod;


    void Awake()
    {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.plane = this.GetComponent<MeshFilter>().mesh;
        Debug.Log("added plane");
        this.myCollider = GetComponent<MeshCollider>();
        Debug.Log("seed x : " + seeds.x + ",  y : " + seeds.y);
        generatePerlinHill();
    }


    public override void OnStartServer()
    {
        initColiders();
        randomizeSeed();
        randomizeSpawnSeeds();
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

    public void randomizeSpawnSeeds()
    {
        SpawnCarTwo = UnityEngine.Random.Range(0,plane.vertices.Length);
        SpawnCarOne = UnityEngine.Random.Range(0,plane.vertices.Length);
        Debug.Log(SpawnCarOne);
    }



    // actually make   
    public void generatePerlinHill()
    {
        Vector3[] vertices = plane.vertices;
      

        for (int i = 0; i< vertices.Length; i++)
        {
            float px = (transform.position.x + vertices[i].x) / quality;
            float pz = (transform.position.z + vertices[i].z) / quality;

            vertices[i].y = Mathf.PerlinNoise(px + seeds.x, pz + seeds.y) * maxHight - maxHight / 2;
            vertices[i].x *= xMod;
            vertices[i].z *= yMod;
        }

        
        CarSpawn.position = new Vector3(0f,3f,0f) + vertices[SpawnCarOne];
        Car2Spawn.position = new Vector3(0f,3f,0f) + vertices[SpawnCarTwo];


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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerCube : MonoBehaviour
{
    MeshCollider meshCollider;
    Rigidbody rigidbody;
    Collision collision;

    [SerializeField]
    private Vector3 cubeSize = Vector3.one;

    [SerializeField]
    private int meshIndex = 0;

    [SerializeField]
    private int meshSize = 6;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        RenderCube();
        SpawnRandom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 CubeSize
    {
        get { return cubeSize; }
        set { cubeSize = value; }
    }

    private void RenderCube()
    {

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();

        meshFilter.mesh = CreateCube();
        meshRenderer.materials = MaterialsList().ToArray();
        meshCollider.sharedMesh = meshFilter.mesh;
        meshCollider.convex = true;

    }

    private Mesh CreateCube()
    {

        MeshGenerator meshGenerator = new MeshGenerator(meshSize);


        //top points
        Vector3 topPoint1 = new Vector3(cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 topPoint2 = new Vector3(-cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 topPoint3 = new Vector3(-cubeSize.x, cubeSize.y, cubeSize.z);
        Vector3 topPoint4 = new Vector3(cubeSize.x, cubeSize.y, cubeSize.z);

        //bottom points
        Vector3 bottomPoint1 = new Vector3(cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 bottomPoint2 = new Vector3(-cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 bottomPoint3 = new Vector3(-cubeSize.x, -cubeSize.y, cubeSize.z);
        Vector3 bottomPoint4 = new Vector3(cubeSize.x, -cubeSize.y, cubeSize.z);


        //top face
        meshGenerator.CreateTriangle(topPoint1, topPoint2, topPoint3, 0);
        meshGenerator.CreateTriangle(topPoint1, topPoint3, topPoint4, 0);

        //bottom face
        meshGenerator.CreateTriangle(bottomPoint3, bottomPoint2, bottomPoint1, 1);
        meshGenerator.CreateTriangle(bottomPoint4, bottomPoint3, bottomPoint1, 1);

        //left face
        meshGenerator.CreateTriangle(bottomPoint2, topPoint3, topPoint2, 2);
        meshGenerator.CreateTriangle(bottomPoint2, bottomPoint3, topPoint3, 2);

        //right face
        meshGenerator.CreateTriangle(bottomPoint4, topPoint1, topPoint4, 3);
        meshGenerator.CreateTriangle(bottomPoint4, bottomPoint1, topPoint1, 3);

        //back face
        meshGenerator.CreateTriangle(bottomPoint3, topPoint4, topPoint3, 4);
        meshGenerator.CreateTriangle(bottomPoint3, bottomPoint4, topPoint4, 4);

        //front face
        meshGenerator.CreateTriangle(bottomPoint1, bottomPoint2, topPoint2, 5);
        meshGenerator.CreateTriangle(bottomPoint1, topPoint2, topPoint1, 5);

        return meshGenerator.CreateMesh();
    }


    private List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material yellowMaterial = new Material(Shader.Find("Specular"));
        yellowMaterial.color = Color.yellow;
        
        materialsList.Add(yellowMaterial);
        materialsList.Add(yellowMaterial);
        materialsList.Add(yellowMaterial);
        materialsList.Add(yellowMaterial);
        materialsList.Add(yellowMaterial);
        materialsList.Add(yellowMaterial);

        return materialsList;
    }

    private void SpawnRandom()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(-28, 28), -1, Random.Range(-28, 28));
        transform.position = spawnPoint;
        Debug.Log(spawnPoint);

        
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision detected");
        this.collision = collision;
    }
    private void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
    }
}

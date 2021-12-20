using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FinishPoint : MonoBehaviour
{
    Rigidbody rigidbody;
    MeshCollider meshCollider;
    [SerializeField]
    private int subMeshSize = 5;

    [SerializeField]
    private float pyramidSize = .5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        RenderPyramid();
        SpawnRandom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RenderPyramid()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();

        meshFilter.mesh = CreatePyramid();
        meshRenderer.materials = MaterialsList().ToArray();
        meshCollider.sharedMesh = meshFilter.mesh;
        meshCollider.convex = true;
    }

    private Mesh CreatePyramid()
    {
        MeshGenerator meshGenerator = new MeshGenerator(subMeshSize);

        // Define points
        Vector3 topPoint = new Vector3(0, pyramidSize, 0);

        Vector3 bottomPoint1 = new Vector3(-pyramidSize, 0, pyramidSize);
        Vector3 bottomPoint2 = new Vector3(-pyramidSize, 0, -pyramidSize);
        Vector3 bottomPoint3 = new Vector3(pyramidSize, 0, -pyramidSize);

        //Create Triangles
        meshGenerator.CreateTriangle(bottomPoint1, bottomPoint2, bottomPoint3, 0);

        meshGenerator.CreateTriangle(topPoint, bottomPoint1, bottomPoint3, 1);
        meshGenerator.CreateTriangle(topPoint, bottomPoint2, bottomPoint1, 2);
        meshGenerator.CreateTriangle(topPoint, bottomPoint3, bottomPoint2, 3);

        return meshGenerator.CreateMesh();
    }

    private List<Material> MaterialsList()
    {
        List<Material> materials = new List<Material>();
        Material redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = Color.red;

        materials.Add(redMaterial);
        materials.Add(redMaterial);
        materials.Add(redMaterial);
        materials.Add(redMaterial);

        return materials;
    }

    private void SpawnRandom()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(-28, 28), -1, Random.Range(-28, 28));
        transform.position = spawnPoint;
        Debug.Log(spawnPoint);


        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionExit(Collision collision)
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collision");
            SceneManager.LoadScene("Task2-Terrain");
        }
    }
}
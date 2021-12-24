using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Track : MonoBehaviour
{

    [SerializeField]
    private int submeshCount = 3;

    [SerializeField]
    private float radius = 50f;

    [SerializeField]
    private float roadMarkerWidth = 0.2f;

    [SerializeField]
    private float roadWidth = 5.0f;

    [SerializeField]
    private float barrierWidth = 0.6f;

    [SerializeField]
    private int quadCount = 300;

    [SerializeField]
    private float minVariance = 300f;
    [SerializeField]
    private float maxVariance = 700f;

    [SerializeField]
    private float minVarianceScale = 3f;
    [SerializeField]
    private float maxVarianceScale = 6f;

    [SerializeField]
    private Vector2 varianceOffset;

    [SerializeField]
    private Vector2 varianceStep = new Vector2(0.01f,0.01f);

    private MeshGenerator meshGenerator;

    [SerializeField]
    private GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        RenderTrack();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RenderTrack()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        meshFilter.mesh = GenerateTrack();
        meshRenderer.materials = MaterialsList().ToArray();
        meshCollider.sharedMesh = meshFilter.mesh;
    }

    private Mesh GenerateTrack()
    {
        meshGenerator = new MeshGenerator(submeshCount);

        // generate track code
        float quadDistance = 360f / quadCount;

        List<Vector3> pointRefList = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += quadDistance)
        {

            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            pointRefList.Add(point);
        }

        //Add Noise to our points to create randomness/curves to our track
        Vector2 curve = varianceOffset;

        float varianceScale = Random.Range(minVarianceScale, maxVarianceScale);
        float variance = Random.Range(minVariance, maxVariance);

        Debug.Log("Variance: " + variance + "\t Variance Scale: "+varianceScale);

        for (int i = 0; i < pointRefList.Count; i++)
        {
            curve += varianceStep;
            Vector3 pointRef = pointRefList[i].normalized;
            float perlinNoise = Mathf.PerlinNoise(curve.x*varianceScale,curve.y*varianceScale);
            perlinNoise *= variance;

            float fix = Mathf.PingPong(i,pointRefList.Count/2f)/(pointRefList.Count/2f);


            pointRefList[i] += pointRef * perlinNoise * fix;
        }

        for (int i = 1; i <= pointRefList.Count; i++)
        {
            Vector3 prevQuad = pointRefList[i - 1];
            Vector3 currQuad = pointRefList[i % pointRefList.Count];
            Vector3 nextQuad = pointRefList[(i + 1) % pointRefList.Count];

            CreateTrack(prevQuad, currQuad, nextQuad);
        }
        int startPosition = 0;
        car.transform.position = pointRefList[startPosition];
        car.transform.LookAt(pointRefList[startPosition++]);
        
        return meshGenerator.CreateMesh();
    }

    private void CreateTrack(Vector3 prevQuad, Vector3 currQuad, Vector3 nextQuad)
    {

        //create the road marker
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * roadMarkerWidth;
        CreateQuad(prevQuad, currQuad, nextQuad, 0, offset, targetOffset);

        //create the road
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;
        CreateQuad(prevQuad, currQuad, nextQuad, 1, offset, targetOffset);

        //create the barrier
        offset += targetOffset;
        targetOffset = Vector3.forward * barrierWidth;
        Vector3 barrierOffset = new Vector3(targetOffset.x, targetOffset.y + 1, targetOffset.z); //Raise barrier to create wall
        CreateQuad(prevQuad, currQuad, nextQuad, 2, offset, barrierOffset);

    }

    private void CreateQuad(Vector3 prevQuad, Vector3 currQuad, Vector3 nextQuad,
                            int submesh, Vector3 offset, Vector3 targetOffset)
    {
        //Outer Side
        Vector3 nextDirection = (nextQuad - currQuad).normalized;
        Vector3 prevDirection = (currQuad - prevQuad).normalized;

        Quaternion nextQuaternion = Quaternion.LookRotation(Vector3.Cross(nextDirection, Vector3.up));
        Quaternion prevQuaternion = Quaternion.LookRotation(Vector3.Cross(prevDirection, Vector3.up));

        Vector3 topLeftPoint = currQuad + (prevQuaternion * offset);
        Vector3 topRightPoint = currQuad + (prevQuaternion * (offset + targetOffset));

        Vector3 bottomLeftPoint = nextQuad + (nextQuaternion * offset);
        Vector3 bottomRightPoint = nextQuad + (nextQuaternion * (offset + targetOffset));

        meshGenerator.CreateTriangle(topLeftPoint, topRightPoint, bottomLeftPoint, submesh);
        meshGenerator.CreateTriangle(topRightPoint, bottomRightPoint, bottomLeftPoint, submesh);

        //inner Side
        nextQuaternion = Quaternion.LookRotation(Vector3.Cross(-nextDirection, Vector3.up));
        prevQuaternion = Quaternion.LookRotation(Vector3.Cross(-prevDirection, Vector3.up));

        topLeftPoint = currQuad + (prevQuaternion * offset);
        topRightPoint = currQuad + (prevQuaternion * (offset + targetOffset));

        bottomLeftPoint = nextQuad + (nextQuaternion * offset);
        bottomRightPoint = nextQuad + (nextQuaternion * (offset + targetOffset));

        meshGenerator.CreateTriangle(bottomLeftPoint, bottomRightPoint, topLeftPoint, submesh);
        meshGenerator.CreateTriangle(bottomRightPoint, topRightPoint, topLeftPoint, submesh);

    }

    private List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        materialsList.Add(whiteMat);
        materialsList.Add(blackMat);
        materialsList.Add(redMat);

        return materialsList;

    }
}

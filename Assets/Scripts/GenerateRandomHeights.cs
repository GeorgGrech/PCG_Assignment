using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainTextureData
{
    public Texture2D terrainTexture;
    public Vector2 tileSize;
    public float minHeight;
    public float maxHeight;
}

[System.Serializable]
public class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}

/*
[System.Serializable]
public class RockData
{
    public GameObject rockMesh;
    public float minHeight;
    public float maxHeight;
}*/

/*
[System.Serializable]
public class GrassData
{
    public Texture grassTexture;
    //public float minHeight;
    //public float maxHeight;
}
*/

/*
[System.Serializable]
public class Player
{
    public GameObject playerPrefab;
    public float minHeight;
    public float maxHeight;
    public float spawnOffset; //Vertical offset to avoid clipping terrain when spawning
}*/

public class GenerateRandomHeights : MonoBehaviour
{
    private Terrain terrain;

    private TerrainData terrainData;

    [SerializeField]
    [Range(0f, 1f)]
    private float minRandomHeightRange = 0;//min is 0

    [SerializeField]
    [Range(0f, 1f)]
    private float maxRandomHeightRange = 0.1f;//max is 1

    [SerializeField]
    private bool flatternTerrain = true;

    [Header("Perlin noise")]
    /*[SerializeField]
    private bool perlinNoise = false;*/

    //To randomly select perlin noise scale between min and max
    [SerializeField] private float minPerlinNoiseWidthScale = 0.002f;
    [SerializeField] private float maxPerlinNoiseWidthScale = 0.02f;

    [SerializeField] private float minPerlinNoiseHeightScale = 0.002f;
    [SerializeField] private float maxPerlinNoiseHeightScale = 0.02f;

    [Header("Texture Data")]
    [SerializeField]
    private List<TerrainTextureData> terrainTextureData;

    [SerializeField]
    private bool addTerrainTexture = false;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [Header("Tree Data")]
    [SerializeField]
    private List<TreeData> treeData;

    [SerializeField]
    private int maxTrees = 1000;

    [SerializeField]
    private int treeSpacing = 20;

    [SerializeField]
    private bool addTrees = false;

    [SerializeField]
    private int terrainLayerIndex;

    [Header("Rock Data")]
    [SerializeField]
    private List<TreeData> rockData;

    [SerializeField]
    private int maxRocks = 1000;

    [SerializeField]
    private int rockSpacing = 20;

    [SerializeField]
    private bool addRocks = false;

    /*[SerializeField]
    private int terrainLayerIndex;*/

    /*
    [Header("Grass Data")]
    [SerializeField]
    private List<GrassData> grassData;

    [SerializeField]
    private int grassAmount = 10000;

    [SerializeField]
    private float grassSpacing = .5f;
    [SerializeField]
    public float minHeightGrass;
    [SerializeField]
    public float maxHeightGrass;

    [SerializeField]
    private bool addGrass = false;*/

    [Header("Water")]
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private float waterHeight = 0.3f;

    [Header("Clouds")]
    [SerializeField]
    private GameObject clouds;
    [SerializeField]
    private float cloudHeight = 0.9f;
    [SerializeField]
    private int cloudAmount = 20;
    [SerializeField]
    private bool addClouds = false;
    [SerializeField]
    private bool addSnow = false;

    [Header("Player")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    public float minPlayerHeight;
    [SerializeField]
    public float maxPlayerHeight;
    [SerializeField]
    public float spawnOffset; //Vertical offset to avoid clipping terrain when spawning

    [Header("Path")]
    [SerializeField]
    private GameObject pathPrefab;
    [SerializeField]
    private int pathLength;
    [SerializeField]
    private float pathOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

        GenerateHeights();
        AddTerrainTextures();
        AddTrees();
        //AddGrass();
        AddWater();
        AddClouds();
        SpawnPlayer();
        //AddPath();
    }

    private void GenerateHeights()
    {

        // var perlinNoiseWidthScale = UnityEngine.Random.Range(0.00f, 0.02f);
        // var perlinNoiseHeightScale = UnityEngine.Random.Range(0.00f, 0.02f);

        float perlinWidth = Random.Range(minPerlinNoiseWidthScale, maxPerlinNoiseWidthScale);
        float perlinHeight = Random.Range(minPerlinNoiseHeightScale, maxPerlinNoiseHeightScale);

        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                /*
                if (perlinNoise)
                {

                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinWidth, height * perlinHeight);
                }
                else
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }
                */

                //heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                heightMap[width, height] += Mathf.PerlinNoise(width * perlinWidth, height * perlinHeight);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    private void FlattenTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] = 0;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    private void AddTerrainTextures()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];

        for (int i = 0; i < terrainTextureData.Count; i++)
        {
            if (addTerrainTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;

        //Bends Textures
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphamapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] alphamap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureData.Count; i++)
                {
                    float heightBegin = terrainTextureData[i].minHeight - terrainTextureBlendOffset;
                    float heightEnd = terrainTextureData[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= heightBegin && heightMap[width, height] <= heightEnd)
                    {
                        alphamap[i] = 1;
                    }
                }

                Blend(alphamap);

                for (int j = 0; j < terrainTextureData.Count; j++)
                {
                    alphamapList[width, height, j] = alphamap[j];
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, alphamapList);
    }

    private void Blend(float[] alphamap)
    {
        float total = 0;

        for (int i = 0; i < alphamap.Length; i++)
        {
            total += alphamap[i];
        }

        for (int i = 0; i < alphamap.Length; i++)
        {
            alphamap[i] = alphamap[i] / total;
        }
    }

    private void AddTrees()
    {
        TreePrototype[] trees = new TreePrototype[treeData.Count];

        for (int i = 0; i < treeData.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeData[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if (addTrees)
        {
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for (int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {
                        if (treeInstanceList.Count < maxTrees)
                        {
                            Vector3 spawnPos = new Vector3(x, terrainData.size.y, z);

                            float currentHeight = Terrain.activeTerrain.SampleHeight(spawnPos) / terrainData.size.y; //give us a height value between 0 & 1

                            if (currentHeight >= treeData[treeIndex].minHeight && currentHeight <= treeData[treeIndex].maxHeight)
                            {
                                float randomX = (x + Random.Range(-5.0f, 5.0f)) / terrainData.size.x;
                                float randomZ = (z + Random.Range(-5.0f, 5.0f)) / terrainData.size.z;

                                Vector3 treePosition = new Vector3(randomX * terrainData.size.x,
                                                                   currentHeight * terrainData.size.y,
                                                                   randomZ * terrainData.size.z) + this.transform.position;

                                RaycastHit raycastHit;

                                int layerMask = 1 << terrainLayerIndex;

                                if (Physics.Raycast(treePosition, -Vector3.up, out raycastHit, 100, layerMask) ||
                                    Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeDistance = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    TreeInstance treeInstance = new TreeInstance();

                                    treeInstance.position = new Vector3(randomX, treeDistance, randomZ);
                                    treeInstance.rotation = UnityEngine.Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            }
        }
        terrainData.treeInstances = treeInstanceList.ToArray();
    }


    /*
    private void AddGrass()
    {
        DetailPrototype[] grass = new DetailPrototype[grassData.Count];

        for (int i = 0; i < grassData.Count; i++)
        {
            grass[i] = new DetailPrototype();
            grass[i].prototypeTexture = (Texture2D)grassData[i].grassTexture;
            grass[i].renderMode = (DetailRenderMode)2; //Render as grass
        }

        terrainData.detailPrototypes = grass;
        terrainData.RefreshPrototypes();
        
        var map = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, terrainLayerIndex);

        for (int y = 0; y < terrainData.detailHeight; y++)
        {
            for (int x = 0; x < terrainData.detailWidth; x++)
            {
                // If the pixel value is below the threshold then
                // set it to zero.
                if (map[x, y] < minHeightGrass || map[x, y] > maxHeightGrass)
                {
                    map[x, y] = 0;
                }
            }
        }

        // Assign the modified map back.
        terrainData.SetDetailLayer(0, 0, 0, map);
    }*/

    private void AddWater()
    {
        GameObject waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
        waterGameObject.name = "Water";
        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y, terrainData.size.z / 2);
        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    private void SpawnPlayer()
    {

        bool playerSpawned = false;

        Vector3 playerPosition = Vector3.zero; //placeholder values
        //Quaternion playerRotation = Quaternion.identity;
        while (!playerSpawned)
        {

            int randomXPos = 0;
            int randomZPos = 0;
            float heightFound = 0;
            bool positionAvailable = false;

            while (!positionAvailable)
            {
                randomXPos = Random.Range(0, (int)terrainData.size.x);
                randomZPos = Random.Range(0, (int)terrainData.size.z);
                Vector3 spawnPos = new Vector3(randomXPos, terrainData.size.y, randomZPos);

                heightFound = Terrain.activeTerrain.SampleHeight(spawnPos)/terrainData.size.y;
                if (heightFound >= minPlayerHeight && heightFound <= maxPlayerHeight)
                {
                    positionAvailable = true;
                    Debug.Log("Height selected: " + heightFound);
                }
            }


            //float playerX = randomXPos / terrainData.size.x;
            //float playerZ = randomZPos / terrainData.size.z;

            playerPosition = new Vector3(randomXPos,
                                                heightFound * terrainData.size.y+spawnOffset,
                                                randomZPos) + this.transform.position;

            Debug.Log("Player spawn position: "+playerPosition);
            Debug.Log("Terrain height " + terrainData.size.y);
            //RaycastHit raycastHit;

            GameObject playerInstance = Instantiate(playerPrefab, playerPosition, this.transform.rotation);

            playerInstance.transform.LookAt(new Vector3(terrainData.size.x/2, playerInstance.transform.position.y, terrainData.size.z/2)); //Look at centre of map, but leave player standing still
            playerSpawned = true;
        }

        playerPosition.y -= spawnOffset; //remove offset for path spawning
        AddPath(playerPosition);
    }

    public void AddClouds()
    {
        if (addClouds)
        {
            for (int x = 0; x < cloudAmount; x++)
            {
                float randomXPos = Random.Range(0, (int)terrainData.size.x);
                float randomZPos = Random.Range(0, (int)terrainData.size.z);
                Vector3 spawnPos = new Vector3(randomXPos, cloudHeight * terrainData.size.y, randomZPos) + this.transform.position;
                GameObject cloud = Instantiate(clouds, spawnPos, Quaternion.identity);

                if (addSnow)
                {
                    float snowChance = Random.Range(0, 9);
                    if (snowChance < 3) //1/3 chance for snow
                    {
                        cloud.transform.Find("Snow").gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void AddPath(Vector3 startPosition)
    {
        /*
        float randomXPos = Random.Range(0, (int)terrainData.size.x);
        float randomZPos = Random.Range(0, (int)terrainData.size.z);

        float startHeight = Terrain.activeTerrain.SampleHeight(new Vector3(randomXPos, terrainData.size.y, randomZPos));
        Vector3 startPos = new Vector3(randomXPos, startHeight, randomZPos);*/

        //Vector3 startPos = startPosition;

        GameObject prevInstance = Instantiate(pathPrefab,startPosition,Quaternion.identity);
        prevInstance.name = "PathOrigin";
        GameObject nextInstance;
        for (int x = 0; x < pathLength; x++)
        {
            nextInstance = Instantiate(pathPrefab,prevInstance.transform); //instantiate at same position
            //nextInstance.transform.SetParent(prevInstance.transform); //reattach to parent for path deviation from rotation

            nextInstance.name = "Path";

            nextInstance.transform.position += new Vector3(0, 0, pathOffset); //move forward

            float randomRotation = Random.Range(-50, 50);
            prevInstance.transform.Rotate(new Vector3(0, randomRotation, 0),Space.Self); //rotate

            //nextInstance.transform.parent = null; //detach from parent to treat it independently
            float fixedHeight = Terrain.activeTerrain.SampleHeight(new Vector3(nextInstance.transform.position.x, terrainData.size.y, nextInstance.transform.position.z));
            nextInstance.transform.position = new Vector3(nextInstance.transform.position.x, fixedHeight, nextInstance.transform.position.z); //change y to proper height on terrain
            //nextInstance.transform.SetParent(prevInstance.transform); //reattach to parent for path deviation from rotation

            prevInstance = nextInstance; //start from new on next iteration
        }
    }


    private void OnDestroy()
    {
        if (flatternTerrain)
        {
            FlattenTerrain();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField]
    //private int wallSize = 6;
    private int wallCounter = 0;
    private float mazeArea = 30;
    private float offset = .5f;
    // Start is called before the first frame update
    void Start()
    {
        //CreateWall();
        CreateBase();
        CreateBorderWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void CreateBase()   
    {
        GameObject cube = new GameObject();
        cube.name = "base";
        Cube c = cube.AddComponent<Cube>();
        cube.transform.position = new Vector3(0,-4.2f, 0);
        cube.transform.parent = this.transform;
        c.CubeSize = new Vector3(mazeArea, 0.15f, mazeArea);
    }

    private void CreateWall(int wallSize,Vector3 startPosition,bool horizontalOrientation)
    {

        Vector3 nextPosition = startPosition;
        GameObject wallParent = new GameObject();
        wallParent.name = "Wall" + wallCounter;
        for (int i = 0; i < wallSize; i++)
        {
            GameObject cube = new GameObject();
            cube.name = "Cube" + i;
            Cube c = cube.AddComponent<Cube>();
            cube.transform.position = nextPosition;
            cube.transform.parent = wallParent.transform;
            c.CubeSize = new Vector3(1, 4, 1);

            if (horizontalOrientation)
                nextPosition.x = nextPosition.x + c.CubeSize.x * 2;
            else
                nextPosition.z = nextPosition.z + c.CubeSize.z * 2;
        }
        wallParent.transform.parent = this.transform;
        wallCounter++;
    }

    private void CreateBorderWalls()
    {
        float offsetWalls = mazeArea - offset;

        CreateWall(30, new Vector3(-offsetWalls, 0, -offsetWalls), true);
        CreateWall(30, new Vector3(-offsetWalls, 0, -offsetWalls), false);
        CreateWall(30, new Vector3(-offsetWalls, 0, offsetWalls), true);
        CreateWall(30, new Vector3(offsetWalls, 0, -offsetWalls), false);
    }
}

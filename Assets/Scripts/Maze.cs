using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField]
    //private int wallSize = 6;
    private int wallCounter = 0;
    private int mazeArea = 30;
    private float offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        //CreateWall();
        CreateBase();
        CreateBorderWalls();
        CreateMazeInterior();
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
                nextPosition.x += c.CubeSize.x * 2;
            else
                nextPosition.z += c.CubeSize.z * 2;
        }
        wallParent.transform.parent = this.transform;
        wallCounter++;
    }

    private void CreateBorderWalls()
    {
        float offsetWalls = mazeArea - offset;

        CreateWall(mazeArea, new Vector3(-offsetWalls, 0, -offsetWalls), true);
        CreateWall(mazeArea, new Vector3(-offsetWalls, 0, -offsetWalls), false);
        CreateWall(mazeArea, new Vector3(-offsetWalls, 0, offsetWalls), true);
        CreateWall(mazeArea, new Vector3(offsetWalls, 0, -offsetWalls), false);
    }

    private void CreateMazeInterior()
    {
        /*
        CreateWall(20, new Vector3(-mazeArea, 0, 10), true);
        CreateWall(6, new Vector3(-10, 0, 10), false);
        CreateWall(10, new Vector3(-15, 0, -10), false);
        CreateWall(7, new Vector3(-mazeArea, 0, 20), true);
        CreateWall(5, new Vector3(-25, 0, 0), true);
        */

        CreateWall(12, new Vector3(-mazeArea, 0, 0), true);
        CreateWall(11, new Vector3(1, 0, 0), true);
        CreateWall(12, new Vector3(0, 0, -mazeArea), false);
        CreateWall(12, new Vector3(0, 0, 8), false);

        CreateWall(6, new Vector3(-20, 0, 2), false);
        CreateWall(7, new Vector3(-10, 0, 16), false);
        CreateWall(5, new Vector3(-10, 0, 8), true);
        CreateWall(5, new Vector3(-20, 0, 20), true);

        CreateWall(5, new Vector3(-mazeArea, 0, -8), true);
        CreateWall(7, new Vector3(-14, 0, -8), true);
        CreateWall(8, new Vector3(-mazeArea, 0, -19), true);
        CreateWall(6, new Vector3(-10, 0, -24), false);

        CreateWall(7, new Vector3(2, 0, -19), true);
        CreateWall(5, new Vector3(21, 0, -10), false);
        CreateWall(5, new Vector3(10, 0, -17), false);
        CreateWall(6, new Vector3(21, 0, -28), false);

        CreateWall(5, new Vector3(10, 0, 2), false);
        CreateWall(6, new Vector3(19, 0, 10), true);
        CreateWall(3, new Vector3(2, 0, 18), true);
        CreateWall(3, new Vector3(13, 0, 18), true);
        CreateWall(6, new Vector3(19, 0, 12), false);



    }
}

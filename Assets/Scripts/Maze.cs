using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField]
    private int wallSize = 6;

    // Start is called before the first frame update
    void Start()
    {
        CreateWall();
        CreateBase();
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
        cube.transform.position = new Vector3(4.5f, -1.2f, 0);
        cube.transform.parent = this.transform;
        c.CubeSize = new Vector3(10, 0.15f, 10);
    }

    private void CreateWall()
    {

        Vector3 nextPosition = Vector3.zero;

        for (int i = 0; i < wallSize; i++)
        {
            GameObject cube = new GameObject();
            cube.name = "Cube" + i;
            Cube c = cube.AddComponent<Cube>();
            cube.transform.position = nextPosition;
            cube.transform.parent = this.transform;
            nextPosition.x = nextPosition.x + c.CubeSize.x * 2;
        }
    }
}

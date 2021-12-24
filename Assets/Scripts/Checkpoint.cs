using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Player"));
        {
            Debug.Log("Car Collision");
            GameManager.GetComponent<GameManager>().passedCheckpoints++;
            Debug.Log(GameManager.GetComponent<GameManager>().passedCheckpoints);
            Destroy(gameObject);
        }
    }
}

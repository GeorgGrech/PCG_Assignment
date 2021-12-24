using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject track;
    private GameObject instantiatedTrack;
    public int trackNum = 0;
    public int passedCheckpoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if(passedCheckpoints >= 2)
        {
            if(trackNum != 3) //if not final track
            {
                DestroyTrack();
                GenerateTrack();
            }
            else //if finished final track
            {
                SceneManager.LoadScene("EndScene");
            }
        }
    }

    public void GenerateTrack()
    {
        passedCheckpoints = 0;
        trackNum++;
        instantiatedTrack = Instantiate(track);
    }

    public void DestroyTrack()
    {
        Destroy(instantiatedTrack);
    }
}

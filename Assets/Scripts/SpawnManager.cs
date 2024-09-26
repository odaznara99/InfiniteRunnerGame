using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public float startDelay, repeatRate;
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(30, 0, 0);
    private bool isSpawningStop = false;

    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle() 
    {
        //If not Game Over, spawn obstacles
        if(playerControllerScript.gameOver == false && !isSpawningStop)
        {
            int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomObstacle], spawnPos, obstaclePrefabs[randomObstacle].transform.rotation);
        }       
    }

    public void StopSpawning(bool stop) {
        isSpawningStop = stop;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;
    private float leftBound = -30;
    private float groundSpeed;
    private PlayerController playerControllerScript;
    private GameManager gameManager;
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameObject.CompareTag("Ground") || gameObject.CompareTag("Background"))
        {
            groundSpeed = speed;
        }
        else 
        { 
            speed = speed * gameManager.obsSpeedMultiplier;
        }
               
    }

    // Update is called once per frame
    void Update()

    {


        //If not game over, move objects to left
        if(playerControllerScript.gameOver == false) 
        {
            if (playerControllerScript.doubleSpeed)
            {
                transform.Translate(Vector3.left * Time.deltaTime * (speed * 2));
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }

        }
        //if the obstacle go out of bounds, destroy obstacle
        if (transform.position.x < leftBound && (gameObject.CompareTag("Obstacle")|| gameObject.CompareTag("Coin"))) {
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Ground") || gameObject.CompareTag("Background")) {
            speed = groundSpeed * gameManager.obsSpeedMultiplier;
        
        
        }
    }
}

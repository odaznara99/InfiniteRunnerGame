using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;
    private float leftBound = -30;
    private PlayerController playerControllerScript;
    void Start()
    {
        playerControllerScript =
            GameObject.Find("Player").GetComponent<PlayerController>();
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
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) {
            Destroy(gameObject);
        }
    }
}

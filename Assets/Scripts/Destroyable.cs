using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private float knockUpForce = 30;
   
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();//get the RB of the player
            playerRb.velocity = new Vector3(playerRb.velocity.x, knockUpForce, playerRb.velocity.z); //create a knock up effect to the player

            PlayerController playerController = other.gameObject.GetComponent<PlayerController>(); // get player controller
            AudioSource playerAudio = other.gameObject.GetComponent<AudioSource>(); // get the Audio source of the player
            playerAudio.PlayOneShot(playerController.destroySound, 1f);
            Destroy(transform.parent.gameObject); //destroy the parent object
            Destroy(gameObject);

        }

    }

   
}

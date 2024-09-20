using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    private Animator eagleAnim;
    private Rigidbody eagleRb;

    private float flyUpSpeed = 20;
   
    void Start()
    {
        eagleAnim = GetComponent<Animator>();
        eagleRb = GetComponent<Rigidbody>();
        

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            eagleAnim.SetBool("isDead", true);
            eagleRb.useGravity = true;
            
        }
    }

    private void Update()
    {

        if(transform.position.x <= 8)
        {
            transform.Translate(Vector3.up * Time.deltaTime * flyUpSpeed);
        }
        
    }
}

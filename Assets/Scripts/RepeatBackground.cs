using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{

    private Vector3 startPos;
    private float repeatWidth;
    void Start()
    {
        //starting position
        startPos = transform.position;
        //get the width/2 of the box collider
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //if positions is less than start position minus the repeatWidth, reset position
        if (transform.position.x < startPos.x - repeatWidth) 
        {
            transform.position = startPos;
        }
        
    }
}

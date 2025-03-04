using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLineMovement : MonoBehaviour
{
    private float movementSpeed = 10.0f;

    void Update()
    { 
        transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);

        //delete if out of bounds
        if (transform.position.y > 30)
        {
            Destroy(gameObject);
        }
    }
}

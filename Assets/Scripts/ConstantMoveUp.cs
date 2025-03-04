using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMoveUp : MonoBehaviour
{
    public float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        //get reference to moving background
        GameObject snowyBackground = GameObject.FindGameObjectWithTag("snowBackground");

        //get reference to the script on the background
        ScrollingBackground scrollingBackground = snowyBackground.GetComponent<ScrollingBackground>();

        //set speed
        movementSpeed = scrollingBackground.speed;

        //move
        transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);

        //delete if out of bounds
        if (transform.position.y > 40)
        {
            Destroy(gameObject);
        }
    }
}

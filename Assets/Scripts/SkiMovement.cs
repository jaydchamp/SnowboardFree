using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiMovement : MonoBehaviour
{
    public Rigidbody2D rb2D;

    private float moveSpeed = 10.0f;

    private bool hitStatus;
    private GameObject player;

    private Vector2 directionTowardPlay;
    private float angleToPlayer;

    //interval between when the skier recalulates which direction they are moving
    private float updatingCounter;
    private float baseInterval = 1.4f;

    void Start()
    {
        updatingCounter = baseInterval;

        player = GameObject.FindGameObjectWithTag("Player");
        //rb2D = GetComponent<Rigidbody2D>();

        rb2D.velocity = Vector2.zero;
        hitStatus = false;

        directionTowardPlay = ((player.transform.position) - (transform.position)).normalized;
        angleToPlayer = Mathf.Atan2(directionTowardPlay.y, directionTowardPlay.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer-270);


        //Debug.Log(directionTowardPlay);
    }

    void Update()
    {
        //if movement stops skiers leave
        if (ScrollingBackground.Instance.getScrollSpeed() <= 0)
        {
            hitStatus = true;
        }

        //once skier has been hit, they ski off screen
        if (hitStatus)
        {
            rb2D.velocity = Vector2.zero;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

            //if out of bounds delete
            if (transform.position.y < -Screen.height)
            {
                Destroy(gameObject);
            }
        }

        //while skier has not collided with player
        if (!hitStatus)
        {
            //move to player
            transform.Translate(directionTowardPlay * moveSpeed * Time.deltaTime);

            //if counter still counting down
            if (updatingCounter > 0)
            {
                //continue counting
                updatingCounter -= Time.deltaTime;

                //if counter reaches 0
                if(updatingCounter <= 0)
                {
                    //change direction moving in
                    Invoke("ChangeDirection", 0.0f);
                    updatingCounter = baseInterval;
                }
            }
        }
    }

    void ChangeDirection()
    {
        Debug.Log("CHANGED DIRECTION");
        //find player position and direction
        Vector2 playerPos = player.transform.position;
        Vector2 tempDirec = (playerPos - (Vector2)transform.position).normalized;

        directionTowardPlay = tempDirec;

        float tempAngle = Mathf.Atan2(directionTowardPlay.y, directionTowardPlay.x) * Mathf.Rad2Deg;
        angleToPlayer = tempAngle;

        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer - 270);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
            hitStatus = true;
        }

        if (collision.gameObject.tag == "Stone" || collision.gameObject.tag == "Tree")
        {
            Debug.Log("Hit Object");
            Destroy(gameObject);
        }
    }
}

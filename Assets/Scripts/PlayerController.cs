using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public float runSpeed = 8.0f;
    private float moveLimiter = 0.7f;
    private float tempScrollingSpeed;

    private Vector2 tempVelocity;
    private bool canMove = true;
    private bool inPauseMenu = false;
    float horizontal;
    float vertical;

    public PlayerHealth playerHealth;

    SpriteRenderer spriteRenderer;
    public Sprite leftFacing;
    public Sprite frontFacing;
    public Sprite rightFacing;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.freezeRotation = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frontFacing;
        UIManager.Instance.Init();
    }

    void Update()
    {
        //if not in menu open menu 
        if (Input.GetKeyDown(KeyCode.Escape) && !inPauseMenu)
        {
            canMove = false;
            inPauseMenu = true;
            tempScrollingSpeed = ScrollingBackground.Instance.getScrollSpeed();

            tempVelocity = rigidbody2D.velocity;
            rigidbody2D.velocity = Vector2.zero;

            ScrollingBackground.Instance.setScrollSpeed(0);
            UIManager.Instance.OpenPauseMenu();
        }
        //else if already in meny close it
        else if (Input.GetKeyDown(KeyCode.Escape) && inPauseMenu)
        {
            canMove = true;
            inPauseMenu = false;

            rigidbody2D.velocity = tempVelocity;

            ScrollingBackground.Instance.setScrollSpeed(tempScrollingSpeed);
            UIManager.Instance.ClosePauseMenu();
        }

        if (canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (vertical > 0 && horizontal == 0)
        {
            spriteRenderer.sprite = frontFacing;
        }
        if (horizontal < 0)
        {
            spriteRenderer.sprite = leftFacing;
        }
        else if (horizontal > 0)
        {
         spriteRenderer.sprite = rightFacing;
        }
        else if (vertical < 0)
        {
            spriteRenderer.sprite = frontFacing;
        }
        //backFacing?
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (vertical == 0)
            {
                //scrollingBackground.speed = originalScrollSpeed;
            }
            //if player is moving down the mountain AND they are not at max speed
            if (vertical < 0 && ScrollingBackground.Instance.getScrollSpeed() < ScrollingBackground.Instance.getMaxSpeed()) //&& scrollingBackground.speed < (insert max speed here)
            {
                //increment speed by 1
                ScrollingBackground.Instance.setScrollSpeed(ScrollingBackground.Instance.getScrollSpeed() + 0.1f);
            }
            //if player is moving up the mountain AND they are not at minimum speed
            if (vertical > 0 && ScrollingBackground.Instance.getScrollSpeed() > 1)
            {
                ScrollingBackground.Instance.setScrollSpeed(ScrollingBackground.Instance.getScrollSpeed() - 0.1f);
                //scrollingBackground.speed = scrollingBackground.speed - 0.5f;
            }

            //limit diagonal movement
            if(horizontal != 0 && vertical !=0)
            {
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            rigidbody2D.velocity = new Vector2(horizontal * runSpeed, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Stone" || collision.gameObject.tag == "Tree")
        {
            //scrollingBackground.setScrollSpeed(0.0f);
            //scrollingBackground.speed = scrollingBackground.speed - scrollingBackground.speed;
            //need to move the player over slightly and then start the speed back up
            ScrollingBackground.Instance.setScrollSpeed(0.0f);
            playerHealth.TakeDamage();
        }

        if (collision.gameObject.tag == "Skier")
        {
            ScrollingBackground.Instance.setScrollSpeed(0.0f);
            playerHealth.TakeDamage();
        }
    }
}

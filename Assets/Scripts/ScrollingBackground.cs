using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingBackground : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    private Vector3 startPos;
    private float widthOfBackground;

    private float maxSpeed = 30;
    public float speed;

    public TMP_Text speedUI;

    public static ScrollingBackground Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        speed = 10.0f;
    }

    void Update()
    {
        speedUI.text = speed.ToString("F2");

        //only move if speed is greater than 0
        if (speed != 0)
        { 
            transform.Translate(Vector3.up * Time.deltaTime * speed);
            if (transform.position.y > 21)   //startPos.y - widthOfBackground) 
            {
                transform.position = startPos;
            }
        }
    }

    public void setScrollSpeed(float s)
    {
        //Debug.Log("HAS BEEN SET TO" + s);
        speed = s;
        rigidbody2D.velocity = Vector2.zero;

    }

    public float getScrollSpeed() 
    {
        return speed;
    }

    public float getMaxSpeed()
    {
        return maxSpeed;
    }
}

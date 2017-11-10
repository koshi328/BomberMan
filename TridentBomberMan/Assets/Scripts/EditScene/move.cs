using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {


        if (Input.GetKeyDown("down") || Input.GetKeyDown("right"))
        {
            Vector2 tmp = GameObject.Find("wakusen1").transform.position;

            if (tmp.y < 200)
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y + 700);
            }
            else if (tmp.y > 500)
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y - 200);
            }
            else
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y - 100);
            }
        }
        else if (Input.GetKeyDown("up") || Input.GetKeyDown("left"))
        {
            Vector2 tmp = GameObject.Find("wakusen1").transform.position;
            if (tmp.y > 700)
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y - 700);
            }
            else if (tmp.y > 400) 
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y + 200);
            }
            else
            {
                GameObject.Find("wakusen1").transform.position = new Vector2(tmp.x, tmp.y + 100);
            }
        }

    }
}

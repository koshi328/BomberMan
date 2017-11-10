using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("down")|| Input.GetKeyDown("right"))
        {
            Vector2 tmp = GameObject.Find("wakusen2").transform.position;
            Vector2 tmp2 = GameObject.Find("wakusen1").transform.position;

            if (tmp2.y < 200)
            {
                GameObject.Find("wakusen2").transform.position = new Vector2(tmp.x, tmp.y + 500);
            }
            else if (tmp2.y > 700)
            {
                GameObject.Find("wakusen2").transform.position = new Vector2(tmp.x, tmp.y - 500);
            }
      
        }
        else if (Input.GetKeyDown("up") || Input.GetKeyDown("left"))
        {
            Vector2 tmp = GameObject.Find("wakusen2").transform.position;
            Vector2 tmp2 = GameObject.Find("wakusen1").transform.position;
            if (tmp2.y > 600&& tmp2.y < 800)
            {
                GameObject.Find("wakusen2").transform.position = new Vector2(tmp.x, tmp.y + 500);
            }
            else if (tmp2.y > 700)
            {
                GameObject.Find("wakusen2").transform.position = new Vector2(tmp.x, tmp.y - 500);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move3 : MonoBehaviour {
    int count1 = 0;
    int count2 = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x > 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y < 0.0f))
        {
            count1 = 1;
        }

        if (!(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
           GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x < 0.0f ||
           GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y > 0.0f))
        {
            count2 = 1;
        }




        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x > 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y < 0.0f)&&count1==1)
        {
            Vector2 tmp = GameObject.Find("wakusen3").transform.position;
            Vector2 tmp2 = GameObject.Find("wakusen1").transform.position;

            if (tmp2.y > 700)
            {
                GameObject.Find("wakusen3").transform.position = new Vector2(tmp.x, tmp.y + 500);
                count1=0;
            }
            else if (tmp2.y > 500)
            {
                GameObject.Find("wakusen3").transform.position = new Vector2(tmp.x, tmp.y - 500);
                count1=0;
            }
            
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x < 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y > 0.0f)&& count2 == 1)
        {
            Vector2 tmp = GameObject.Find("wakusen3").transform.position;
            Vector2 tmp2 = GameObject.Find("wakusen1").transform.position;
            if (tmp2.y > 400 && tmp2.y < 600)
            {
                GameObject.Find("wakusen3").transform.position = new Vector2(tmp.x, tmp.y + 500);
                count2=0;
            }
            else if (tmp2.y > 600 && tmp2.y < 800)
            {
                GameObject.Find("wakusen3").transform.position = new Vector2(tmp.x, tmp.y - 500);
                count2 = 0;
            }
            
        }
        

    }
}

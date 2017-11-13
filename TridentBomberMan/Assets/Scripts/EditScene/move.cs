using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

    Rigidbody2D rb;

    float _waitTime;
    int count1 = 0;
    int count2 = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x > 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y < 0.0f )&& count1 == 0)
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

            count1++;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x < 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y > 0.0f) && count2 == 0)
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
            count2++;
        }

        if (!(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x > 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y < 0.0f))
        {
            count1 = 0;
        }

        if (!(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).x < 0.0f ||
            GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One).y > 0.0f))
        {
            count2 = 0;
        }

    }
}

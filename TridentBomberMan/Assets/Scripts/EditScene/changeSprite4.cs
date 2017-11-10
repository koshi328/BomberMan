using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSprite4 : MonoBehaviour
{
    public Sprite statusImage0;
    public Sprite statusImage1;
    public Sprite statusImage2;
    public Sprite statusImage3;
    public Sprite statusImage4;


    public int count;
    // Use this for initialization
    void Start()
    {
        count = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Vector2 tmp = GameObject.Find("wakusen1").transform.position;
            if (tmp.y > 150 && tmp.y < 250)
            {
                count++;
                if (count == 5)
                {
                    count = 0;
                }
                Image myImage = GetComponent<Image>();
                switch (count)
                {
                    case 0:
                        {
                            myImage.sprite = statusImage0;
                            break;
                        }
                    case 1:
                        {
                            myImage.sprite = statusImage1;
                            break;
                        }
                    case 2:
                        {
                            myImage.sprite = statusImage2;
                            break;
                        }

                    case 3:
                        {
                            myImage.sprite = statusImage3;
                            break;
                        }

                    case 4:
                        {
                            myImage.sprite = statusImage4;
                            break;
                        }
                }
            }
        }


    }
}


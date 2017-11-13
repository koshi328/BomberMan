
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSceneTitle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Vector2 tmp = GameObject.Find("wakusen1").transform.position;
            if (tmp.y > 650 && tmp.y < 750)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
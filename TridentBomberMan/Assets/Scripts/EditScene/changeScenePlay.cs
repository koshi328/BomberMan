using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScenePlay : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            Vector2 tmp = GameObject.Find("wakusen1").transform.position;
            if (tmp.y > 850 && tmp.y < 950)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
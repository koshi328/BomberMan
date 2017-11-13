using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTest : MonoBehaviour {
    [SerializeField]
    Material myMat;
    float time;
	// Use this for initialization
	void Start () {
        //myMat = GetComponent<Renderer>().material;
        time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        myMat.SetFloat("_Threshold", (Mathf.Sin(time) + 1) / 2);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTestObject : Photon.PunBehaviour {

    PhotonView _photonView;
	// Use this for initialization
	void Start () {
        _photonView = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!_photonView.isMine) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 5;
        transform.position += new Vector3(x, y, 0);
	}
}

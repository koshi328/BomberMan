using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTestObject : Photon.PunBehaviour {

    PhotonView _photonView;

    int FLAG1 = 0x01;
    int FLAG2 = 0x02;
    int FLAG4 = 0x04;
    int FLAG8 = 0x08;

    int status;
    // Use this for initialization
    void Start () {
        _photonView = GetComponent<PhotonView>();
        status = 0x00;
	}
	
	// Update is called once per frame
	void Update () {
        if (!_photonView.isMine) return;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * 5;
        transform.position += new Vector3(x, y, 0);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _photonView.RPC("StatusUpdate", PhotonTargets.AllViaServer);
            Debug.Log(status);
        }
	}

    [PunRPC]
    void StatusUpdate()
    {
        if ((status & FLAG2) != 0)
        {
            status &= ~FLAG2;
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        }
        else
        {
            status = FLAG2;
            GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
        }
    }
}

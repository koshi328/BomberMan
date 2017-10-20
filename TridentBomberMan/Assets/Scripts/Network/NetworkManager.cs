using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings(null);
	}
		
    public override void OnConnectedToMaster()
    {
        Debug.Log("サーバに接続");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに接続");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("default",ro,TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("部屋に入りました");
    }

}

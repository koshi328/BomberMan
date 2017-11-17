using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public int _playerNum;
    public int _humanNum;

	void Awake ()
    {
        DontDestroyOnLoad(this);
	}
	
    public void SetData(int playerNum, int humanNum)
    {
        _playerNum = playerNum;
        _humanNum = humanNum;
    }
}

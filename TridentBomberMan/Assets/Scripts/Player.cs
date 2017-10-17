using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{
    // 状態異常
    public enum STATE
    {
        NORMAL,     // 健康
        QUICK,      // クイック
        SLOW,       // スロー
        CONFUSION,  // 混乱
        STATE_NUM,
    }

    // アクション
    public enum ACTION
    {
        STAY,   // 待機
        MOVE,   // 移動中
        ACTION_NUM,
    }

    public STATE _state
    {
        get;
        set;
    }

    public ACTION _action
    {
        get;
        set;
    }

    void Start ()
    {
        _state = STATE.NORMAL;
        _action = ACTION.STAY;
	}
	
	void Update ()
    {
		
	}
}

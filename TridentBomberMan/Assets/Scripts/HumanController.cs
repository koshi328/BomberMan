using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : ControllerBase
{
	void Start ()
    {
	}
	
	public override void Update ()
    {
        MoveControl();
        SetBombControl();
    }

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void MoveControl()
    {
        // 操作対象が移動中なら処理しない
        if (_player._action == Player.ACTION.MOVE) return;

        base.MoveControl();

        ControllerBase.DIRECTION direction = ControllerBase.DIRECTION.NONE;

        // 入力を取得
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // 縦方向の移動の入力がされていたら
        if (float.Epsilon < Mathf.Abs(yInput))
        {
            if(yInput < 0)
            {
                direction = ControllerBase.DIRECTION.DOWN;
                Debug.Log("Pushed Down Key.");
            }
            else
            {
                direction = ControllerBase.DIRECTION.UP;
                Debug.Log("Pushed Up Key.");
            }
        }
        else if(float.Epsilon < Mathf.Abs(xInput))
        {
            if (xInput < 0)
            {
                direction = ControllerBase.DIRECTION.LEFT;
                Debug.Log("Pushed Left Key.");
            }
            else
            {
                direction = ControllerBase.DIRECTION.RIGHT;
                Debug.Log("Pushed Right Key.");
            }
        }
        else
        {
            return;
        }
        
        // 移動処理
        base.Move(direction);
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void SetBombControl()
    {
        base.SetBombControl();


    }
}

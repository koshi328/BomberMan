using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : ControllerBase
{
	void Start ()
    {
	}

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void ControlMove()
    {
        // 操作対象が移動中なら処理しない
        if (_player._state == Player.STATE.MOVE) return;

        base.ControlMove();

        Vector2 direction = Vector2.zero;

        // 入力を取得
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // 縦方向の移動の入力がされていたら
        if (float.Epsilon < Mathf.Abs(yInput))
        {
            if (yInput < 0)
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.up;
            }
        }
        else if (float.Epsilon < Mathf.Abs(xInput))
        {
            if (xInput < 0)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }
        else
        {
            return;
        }

        //if(Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    direction = Player.DIRECTION.LEFT;
        //}
        //else if(Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    direction = Player.DIRECTION.RIGHT;
        //}
        //else if(Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    direction = Player.DIRECTION.UP;
        //}
        //else if(Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    direction = Player.DIRECTION.DOWN;
        //}
        //else
        //{
        //    return;
        //}

        // 移動処理
        _player.Move(direction);
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        base.ControlSetBomb();

        // ボタンが押されていたら
        if (Input.GetButtonDown("SetBomb"))
        {
            _player.SetBomb();
        }
    }
}

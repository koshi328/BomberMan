using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : ControllerBase
{
	public override void Initialize ()
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

        // 取得するキー情報のタグ
        var horizontal = "Horizontal" + (_player._playerNumber + 1).ToString() + "P";
        var vertical = "Vertical" + (_player._playerNumber + 1).ToString() + "P";

        // 入力を取得
        float xInput = Input.GetAxisRaw(horizontal);
        float yInput = Input.GetAxisRaw(vertical);

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
        if (!_player.GetStatus(Player.ISMISOBON))
        {
            _player.Move(direction);
        }
        else
        {
            _player.MoveMisobon(direction);
        }
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        base.ControlSetBomb();

        var buttonName = "SetBomb" + (_player._playerNumber + 1).ToString() + "P";

        // ボタンが押されていたら
        if (Input.GetButtonDown(buttonName))
        {
            if (!_player.GetStatus(Player.ISMISOBON))
            {
                _player.SetBomb();
            }
            else
            {
                _player.SetBombMisobon();
            }
        }
    }
}

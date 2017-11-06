using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticController : ControllerBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
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

        //// ランダムな数値によって行動を変える
        //int n = Random.Range(0, 10);
        //switch(n)
        //{
        //    case 0:
        //        direction = Vector2.up;
        //        break;
        //    case 1:
        //        direction = Vector2.down;
        //        break;
        //    case 2:
        //        direction = Vector2.right;
        //        break;
        //    case 3:
        //        direction = Vector2.left;
        //        break;
        //}

        _player.Move(direction);
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        if (_player._state == Player.STATE.MOVE) return;

        base.ControlSetBomb();

        //// ランダムな数値によって行動を変える
        //int n = Random.Range(0, 10);
        //if (n == 0)
        //{
        //    _player.SetBomb();
        //}
    }

    
}

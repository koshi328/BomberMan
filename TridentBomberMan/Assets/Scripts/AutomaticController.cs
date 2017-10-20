using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticController : ControllerBase
{
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
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        base.ControlSetBomb();
    }
}

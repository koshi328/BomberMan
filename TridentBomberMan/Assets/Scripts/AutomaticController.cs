using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticController : ControllerBase
{
    void Start()
    {
    }

    public override void Update()
    {
        MoveControl();
        SetBombControl();
    }

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void MoveControl()
    {
        base.MoveControl();
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void SetBombControl()
    {
        base.SetBombControl();
    }
}

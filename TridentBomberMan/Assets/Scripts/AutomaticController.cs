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

        // 今の座標の情報を取得する
        MapController.STATE state = _map.GetChipState(_player.GetPosition().x, _player.GetPosition().y);

        switch(state)
        {
            case MapController.STATE.NONE:
                break;
            case MapController.STATE.CAUTION:
                if (_map.GetChipState(_player.GetPosition().x - 1, _player.GetPosition().y) == MapController.STATE.NONE)
                {
                    direction = Vector2.left;
                }
                else if (_map.GetChipState(_player.GetPosition().x + 1, _player.GetPosition().y) == MapController.STATE.NONE)
                {
                    direction = Vector2.right;
                }
                else if (_map.GetChipState(_player.GetPosition().x, _player.GetPosition().y - 1) == MapController.STATE.NONE)
                {
                    direction = Vector2.down;
                }
                else if (_map.GetChipState(_player.GetPosition().x, _player.GetPosition().y + 1) == MapController.STATE.NONE)
                {
                    direction = Vector2.up;
                }
                // 移動処理
                _player.Move(direction);
                break;
            case MapController.STATE.BOMB:
                if (_map.GetChipState(_player.GetPosition().x - 1, _player.GetPosition().y) == MapController.STATE.NONE)
                {
                    direction = Vector2.left;
                }
                else if (_map.GetChipState(_player.GetPosition().x + 1, _player.GetPosition().y) == MapController.STATE.NONE)
                {
                    direction = Vector2.right;
                }
                else if (_map.GetChipState(_player.GetPosition().x, _player.GetPosition().y - 1) == MapController.STATE.NONE)
                {
                    direction = Vector2.down;
                }
                else if (_map.GetChipState(_player.GetPosition().x, _player.GetPosition().y + 1) == MapController.STATE.NONE)
                {
                    direction = Vector2.up;
                }
                // 移動処理
                _player.Move(direction);
                break;
        }
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        base.ControlSetBomb();
    }

    
}

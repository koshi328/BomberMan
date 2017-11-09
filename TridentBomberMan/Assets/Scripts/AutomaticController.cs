using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticController : ControllerBase
{
    MapNavigation _mapNavigation;
    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        _mapNavigation = GameObject.Find("MapNavigation").GetComponent<MapNavigation>();
    }

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void ControlMove()
    {
        // 操作対象が移動中なら処理しない
        if (_player._state == Player.STATE.MOVE) return;
        if (_mapNavigation.GetCanSafetyBombPut(_player.GetPosition().x, _player.GetPosition().y) <= 0.0f)
        {
                _player.SetBomb();
        }
        else
        {
            Vector2 direction = GetDirection();

            // 混乱状態なら移動方向を逆にする
            if (_player.GetStatus(Player.ISCONFUSION))
            {
                direction.x *= -1.0f;
                direction.y *= -1.0f;
            }

            _player.Move(direction);


            _player.Move(direction);
        }
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

    Vector2 GetDirection()
    {
        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
        };
        int elem = 0;
        for (int i = 0; i < dir.Length; i++)
        {
            int x = _player.GetPosition().x + (int)dir[i].x;
            int y = _player.GetPosition().y + (int)dir[i].y;
            int ex = _player.GetPosition().x + (int)dir[elem].x;
            int ey = _player.GetPosition().y + (int)dir[elem].y;
            if (_map.GetChipState(x, y) != MapController.STATE.NONE) continue;
            if (_mapNavigation.GetScore(x, y) < _mapNavigation.GetScore(ex, ey))
            {
                elem = i;
            }
        }
        int resultX = _player.GetPosition().x + (int)dir[elem].x;
        int resultY = _player.GetPosition().y + (int)dir[elem].y;
        float currentScore = _mapNavigation.GetBombInfluence(_player.GetPosition().x, _player.GetPosition().y);
        float nextScore = _mapNavigation.GetScore(resultX, resultY);
        if ((currentScore <= 0.0) && nextScore >= 0.1f)
        {
            return Vector2.zero;
        }
        return dir[elem];
    }


}

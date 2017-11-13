using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticController : ControllerBase
{
    MapNavigation _mapNavigation;
    BattleManager _battleManager;
    float[,] playersInfluence;
    int width;
    int height;
    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        _mapNavigation = GameObject.Find("MapNavigation").GetComponent<MapNavigation>();
        _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        width = _map.GetWidth();
        height = _map.GetHeight();
        playersInfluence = new float[width, height];
    }

    /// <summary>
    /// 操作に応じて移動処理を呼び出す関数
    /// </summary>
    public override void ControlMove()
    {
        // 操作対象が移動中なら処理しない
        if (_player._state == Player.STATE.MOVE) return;
        CulcPlayerInfluence();
        //bool blockExist = _mapNavigation.ExistBreakableBlock();
        //bool setBomb = (!blockExist) && Random.Range(0, 25) == 0;
        bool setBomb = false;
        if (_mapNavigation.GetCanSafetyBombPut(_player.GetPosition().x, _player.GetPosition().y) <= 0.0f ||
            playersInfluence[_player.GetPosition().x, _player.GetPosition().y] <= 0.0f)
        {
            if(!CheckBomb(_player.GetPosition().x, _player.GetPosition().y))
                setBomb = _player.SetBomb();
        }
        if (!setBomb)
        {
            Vector2 direction = GetDirection();

            // 混乱状態なら移動方向を逆にする
            if (_player.GetStatus(Player.ISCONFUSION))
            {
                direction.x *= -1.0f;
                direction.y *= -1.0f;
            }

            _player.Move(direction);
        }
        //if (setBomb)
        //{
        //    _player.SetBomb();
        //}
    }

    /// <summary>
    /// 操作に応じてボム置き処理を呼び出す関数
    /// </summary>
    public override void ControlSetBomb()
    {
        if (_player._state == Player.STATE.MOVE) return;
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
        int elem_next = 0;
        for (int i = 0; i < dir.Length; i++)
        {
            int x = _player.GetPosition().x + (int)dir[i].x;
            int y = _player.GetPosition().y + (int)dir[i].y;
            int ex = _player.GetPosition().x + (int)dir[elem].x;
            int ey = _player.GetPosition().y + (int)dir[elem].y;
            if (_map.GetChipState(x, y) != MapController.STATE.NONE) continue;
            if (GetScore(x, y) < GetScore(ex, ey))
            {
                elem_next = elem;
                elem = i;
            }
        }
        for (int p = 0; p < _battleManager._playerNum; p++)
        {
            Player player = _battleManager.GetPlayer(p);
            if (player.gameObject.GetActive() == false) continue;
            if (_player == player) continue;
            if(player.GetPosition().x == _player.GetPosition().x && player.GetPosition().y == _player.GetPosition().y)
            {
                elem = Random.Range(0, 4);
            }
        }


        int resultX = _player.GetPosition().x + (int)dir[elem].x;
        int resultY = _player.GetPosition().y + (int)dir[elem].y;
        float currentScore = _mapNavigation.GetBombInfluence(_player.GetPosition().x, _player.GetPosition().y);
        float nextScore = _mapNavigation.GetBombInfluence(resultX, resultY);
        if (currentScore <= 0.0 && nextScore >= 0.3f)
        {
            return Vector2.zero;
        }
        return dir[elem];
    }

    void CulcPlayerInfluence()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                playersInfluence[i, j] = 0;
            }
        }
        for (int p = 0; p < _battleManager._playerNum; p++)
        {
            Player player = _battleManager.GetPlayer(p);
            if (player.gameObject.GetActive() == false) continue;
            if (_player == player) continue;
            Vector2[] dir =
            {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
            };
            for (int i = 0; i < dir.Length; i++)
            {
                for (int j = 2; j <= 20; j++)
                {
                    int x = player.GetPosition().x + ((int)dir[i].x * j);
                    int y = player.GetPosition().y + ((int)dir[i].y * j);
                    if (_map.GetChipState(x, y) != MapController.STATE.NONE)
                    {
                        break;
                    }
                    playersInfluence[x, y] = 1.0f;
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (playersInfluence[i, j] >= 1.0f)
                    BombRouteCulc(i, j);
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                playersInfluence[i, j] = 1.0f - playersInfluence[i, j];
            }
        }
    }

    void BombRouteCulc(int x, int y)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_map.GetChipState(i, j) != MapController.STATE.NONE)
                {
                    continue;
                }
                if (x == i && y == j)
                {
                    playersInfluence[i, j] = 1.0f;
                    continue;
                }
                float range = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                range = 1 - (range / (width + height)) * 2;
                if (playersInfluence[i, j] > range) continue;
                playersInfluence[i, j] = range;
            }
        }
    }

    float GetScore(int x,int y)
    {
        float score = 0.0f;
        score += _mapNavigation.GetScore(x, y) * 0.9f;
        score += playersInfluence[x, y] * 0.1f;
        return score;
    }

    bool CheckBomb(int x, int y)
    {
        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0)
        };
        for (int i = 0; i < dir.Length; i++)
        {
            for (int j = 1; j <= 20; j++)
            {
                int dest_x = x + ((int)dir[i].x * j);
                int dest_y = y + ((int)dir[i].y * j);
                if (_map.GetChipState(dest_x, dest_y) != MapController.STATE.NONE)
                {
                    if (_map.GetChipState(dest_x, dest_y) == MapController.STATE.BOMB)
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return false;
    }
}

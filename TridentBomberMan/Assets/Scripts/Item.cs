using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MapObject
{
    public enum KIND
    {
        SPEED_UP,   // スピードアップ
        FIRE_UP,    // 火力アップ
        FIRE_FULL,  // フル火力
        BOMB_UP,    // ボムの所持数アップ
        BOMB_KICK,  // ボムキックができるようになる
        INVINCIBLE, // ボムを10秒間すり抜けて移動できる　その間ボムキックはできなくなる
        DOKURO,     // 状態異常が発生する　クイック、スロー、混乱　のいずれか

        KIND_NUM,
    }

    // 種類
    [SerializeField]
    private KIND _kind;


    /// <summary>
    /// 初期化
    /// </summary>
	void Start ()
    {
        transform.DOLocalPath(new Vector3[] { new Vector3(transform.position.x, transform.position.y + 0.1f), new Vector3(transform.position.x, transform.position.y) }, 1.0f, PathType.CatmullRom).SetLoops(-1);
    }

    public void InfluenceEffect(Player player)
    {
        switch (_kind)
        {
            case KIND.SPEED_UP:
                if (Player.SPEED_MAX <= player._speedLevel) return;
                player._speedLevel += 1;
                break;
            case KIND.FIRE_UP:
                if (Player.FIRE_MAX <= player._fireLevel) return;
                player._fireLevel += 1;
                break;
            case KIND.FIRE_FULL:
                player._fireLevel = Player.FIRE_MAX;
                break;
            case KIND.BOMB_UP:
                player._maxBombNum = player._maxBombNum + 1;
                player._currentBombNum = player._currentBombNum + 1;
                break;
            case KIND.BOMB_KICK:
                player.SetStatus(Player.KICKABLE, true);
                break;
            case KIND.INVINCIBLE:
                player.SetStatus(Player.INVINCIBLE, true);
                break;
            case KIND.DOKURO:
                int n = Random.Range(1, 4);
                switch (n)
                {
                    case 1:
                        player.SetStatus(Player.SLOW, true);
                        break;
                    case 2:
                        player.SetStatus(Player.QUICK, true);
                        break;
                    case 3:
                        player.SetStatus(Player.ISCONFUSION, true);
                        break;
                }
                break;
        }
    }
}

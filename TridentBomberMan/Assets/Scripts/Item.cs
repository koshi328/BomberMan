﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject
{
    public enum KIND
    {
        SPEED_UP,   // スピードアップ
        FIRE_UP,    // 火力アップ
        FIRE_FULL,  // フル火力
        BOMB_UP,    // ボムの所持数アップ
        BOMB_KICK,  // ボムキックができるようになる
        BOMB_MINE,  // 地雷ボムをいつでも1回だけ設置できるようになる
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
		
	}

    /// <summary>
    /// 生成の時に呼び出してもらう初期化
    /// </summary>
    public void Init(KIND kind)
    {

    }

    public void InfluenceEffect(Player _player)
    {
        switch(_kind)
        {
            case KIND.SPEED_UP:
                _player._speedLevel++;
                break;
            case KIND.FIRE_UP:
                _player._fireLevel++;
                break;
            case KIND.FIRE_FULL:
                _player._fireLevel = Player.FIRE_MAX;
                break;
            case KIND.BOMB_UP:
                _player._maxBombNum++;
                break;
            case KIND.BOMB_KICK:
                _player.SetStatus(Player.KICKABLE, true);
                break;
            case KIND.BOMB_MINE:
                _player.SetStatus(Player.CANSETMINE, true);
                break;
            case KIND.INVINCIBLE:
                _player.SetStatus(Player.INVINCIBLE, true);
                break;
            case KIND.DOKURO:
                int n = Random.Range(1, 3);
                switch(n)
                {
                    case 1:
                        _player.SetStatus(Player.SLOW, true);
                        break;
                    case 2:
                        _player.SetStatus(Player.QUICK, true);
                        break;
                    case 3:
                        _player.SetStatus(Player.ISCONFUSION, true);
                        break;
                }
                break;
        }
    }
}

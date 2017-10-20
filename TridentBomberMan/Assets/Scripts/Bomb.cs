using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MapObject
{
    // 持ち主
    public int _playerNumber { get; set; }

    // 爆発までの時間
    public static readonly float LIMIT_TIME = 3.0f;

    // 爆発までの残り時間
    public float _currentElapsedTime;

    // 火力
    public int _fireLevel;



    /// <summary>
    /// 初期化
    /// </summary>
	void Start ()
    {
        // 爆発までの時間
        _currentElapsedTime = LIMIT_TIME;
    }

    /// <summary>
    /// 生成した時に呼び出してもらう初期化
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fireLevel"></param>
    public void Init(int playerNumber, int x, int y, int fireLevel)
    {
        _playerNumber = playerNumber;
        _position.x = x;
        _position.y = y;
        _fireLevel = fireLevel;

        // 爆発までの時間
        _currentElapsedTime = LIMIT_TIME;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void MyUpdate ()
    {
        // 爆発までのカウントダウン
        _currentElapsedTime -= Time.deltaTime;
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    void Explosion()
    {
        // 爆発のアニメーション開始等をしたりする

    }

    public void SetActive(bool flag)
    {
        gameObject.SetActive(flag);
    }

    public bool GetActive()
    {
        return gameObject.activeSelf;
    }

    public void Detonate()
    {
        _currentElapsedTime = 0.1f;
    }
}

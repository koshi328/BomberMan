using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MapObject
{
    // 爆発までの時間
    public static readonly float LIMIT_TIME = 3.0f;

    // 爆発までの残り時間
    private float _currentElapsedTime;

    // 火力
    private int _fireLevel;

    // マップ情報
    private MapController _map;



    /// <summary>
    /// 初期化
    /// </summary>
	void Start ()
    {
        // 爆発までの時間
        _currentElapsedTime = LIMIT_TIME;
    }
	
    /// <summary>
    /// 更新
    /// </summary>
	void Update ()
    {
        // 爆発までのカウントダウン
        _currentElapsedTime -= Time.deltaTime;

        Explosion();
    }

    /// <summary>
    /// 生成した時に呼び出してもらう初期化
    /// </summary>
    /// <param name="fireLevel"></param>
    /// <param name="map"></param>
    public void Init(int fireLevel, MapController map)
    {
        SetFireLevel(fireLevel);
        SetMap(map);
    }

    /// <summary>
    /// 火力レベルを設定する
    /// </summary>
    void SetFireLevel(int fireLevel)
    {
        _fireLevel = fireLevel;
    }

    /// <summary>
    /// マップを知る
    /// </summary>
    /// <param name="map"></param>
    void SetMap(MapController map)
    {
        _map = map;
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    void Explosion()
    {
        // まだ爆発しないなら処理しない
        if (0 < _currentElapsedTime) return;

        // 爆発処理

    }
}

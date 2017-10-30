using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private MapController _map;
    private BattleManager _battleManager;
    private Animator _myAnimator;

    /// <summary>
    /// 初期化
    /// </summary>
	void Start ()
    {
        // 爆発までの時間
        _currentElapsedTime = LIMIT_TIME;
        _myAnimator = GetComponent<Animator>();
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

        if(_currentElapsedTime <= 1.0f)
        {
            _myAnimator.SetBool("Verge", true);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="map"></param>
    public void Move(Vector2 direction, MapController map, BattleManager battleManager)
    {
        if (!_map) _map = map;
        if (!_battleManager) _battleManager = battleManager;

        // 目的地を計算
        MapController.Position destination = new MapController.Position(_position.x, _position.y);
        destination.x += (int)(direction.x);
        destination.y += (int)(direction.y);

        // チップの情報を取得
        MapController.STATE state = map.GetChipState(destination.x, destination.y);
        
        // 移動可能な場合以外は処理しない
        switch (state)
        {
            case MapController.STATE.NONE:
                break;
            case MapController.STATE.IMMUTABLE_BLOCK:
                return;
            case MapController.STATE.BREAKABLE_BLOCK:
                return;
            case MapController.STATE.BOMB:
                return;
        }

        // プレイヤーの情報を取得
        for (int i = 0; i < battleManager._playerNum; i++)
        {
            Player player = battleManager.GetPlayer(i);

            // プレイヤーに当たったら
            if(player.GetPosition().x == destination.x &&
                player.GetPosition().y == destination.y)
            {
                player.SetStatus(Player.ISSTAN, true);
            }
        }

        // チップが移動可能なマスなら移動
        Vector3 destinationPosition = MapController.GetChipPosition(destination.x, destination.y);
        transform.DOMove(destinationPosition, 0.1f).OnComplete(() => {
            // アニメーションが終了時によばれる
            _map.SetChipState(_position.x, _position.y, MapController.STATE.NONE);
            _map.SetChipState(destination.x, destination.y, MapController.STATE.BOMB);
            SetPosition(destination.x, destination.y, true);
            Move(direction, _map, _battleManager);
        });
    }

    /// <summary>
    /// 移動が完了した時に呼ばれるコールバック
    /// </summary>
    protected void MovedCallback()
    {
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

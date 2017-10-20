using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MapObject
{
    // 移動方向
    public enum DIRECTION
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }

    // 状態異常
    public enum STATE
    {
        NORMAL,     // 健康
        QUICK,      // クイック
        SLOW,       // スロー
        CONFUSION,  // 混乱
        STATE_NUM,
    }

    // アクション
    public enum ACTION
    {
        STAY,   // 待機
        MOVE,   // 移動中
        ACTION_NUM,
    }



    public STATE _state
    {
        get;
        set;
    }

    public ACTION _action
    {
        get;
        set;
    }

    // マップ情報を取得するために持つ
    [SerializeField]
    protected MapController _map;

    [SerializeField]
    private int _fireLevel;



    void Start ()
    {
        _state = STATE.NORMAL;
        _action = ACTION.STAY;
        _fireLevel = 2;
	}

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    public void Move(DIRECTION direction)
    {
        // 現在地を取得
        MapController.Position currentPosition = GetPosition();
        MapController.Position destination = new MapController.Position(0, 0);

        // チップの情報を保存するための変数とりあえず破壊不可能ブロック
        MapController.STATE state = MapController.STATE.IMMUTABLE_BLOCK;

        // 移動方向
        switch (direction)
        {
            case DIRECTION.UP:
                destination.y = 1;
                break;
            case DIRECTION.DOWN:
                destination.y = -1;
                break;
            case DIRECTION.RIGHT:
                destination.x = 1;
                break;
            case DIRECTION.LEFT:
                destination.x = -1;
                break;
            default:
                break;
        }

        // 目的地を計算
        destination.x += currentPosition.x;
        destination.y += currentPosition.y;

        // 目的地のチップ情報を取得する
        state = _map.GetChipState(destination.x, destination.y);
        
        // 移動可能なマスの場合はbreak、不可能なら以下を処理しない為にreturn
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

        // チップが移動可能なマスなら移動
        Vector3 destinationPosition = MapController.GetChipPosition(destination.x, destination.y);
        transform.DOMove(destinationPosition, 0.1f).OnComplete(MovedCallback);
        SetPosition(destination.x, destination.y, true);
        _action = ACTION.MOVE;
    }

    /// <summary>
    /// 移動が完了した時に呼ばれるコールバック
    /// </summary>
    protected void MovedCallback()
    {
        // 操作対象を待機状態にする
        _action = Player.ACTION.STAY;
    }

    /// <summary>
    /// ボムを置く
    /// </summary>
    public void SetBomb()
    {
        _map.SetBomb(GetPosition().x, GetPosition().y, _fireLevel);
    }

    /// <summary>
    /// マップを知る
    /// </summary>
    public void SetMap(MapController map)
    {
        _map = map;
    }
}

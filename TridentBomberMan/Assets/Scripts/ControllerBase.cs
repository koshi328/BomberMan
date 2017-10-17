using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ControllerBase
{
    // 移動方向
    public enum DIRECTION
    {
        NONE,
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }

    // マップ情報を取得するために持つ
    [SerializeField]
    protected MapController _map;

    // 操作する対象
    [SerializeField]
    protected Player _player;

	void Start()
    {

	}
	
	public virtual void Update()
    {
		
	}

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    public void Move(DIRECTION direction)
    {


        // 現在地を取得
        MapController.Position currentPosition = _player.GetPosition();
        MapController.Position destination;
        destination.x = 0;
        destination.y = 0;

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
                Debug.Log("Move.");

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
        _player.transform.DOMove(destinationPosition, 0.1f).OnComplete(MovedCallback);
        _player.SetPosition(destination.x, destination.y, true);
        Debug.Log(destinationPosition);
    }

    /// <summary>
    /// ボムを置く
    /// </summary>
    public void SetBomb()
    {

    }

    /// <summary>
    /// 操作に応じてMove()を呼び出す仮想関数
    /// </summary>
    virtual public void MoveControl()
    {
    }

    /// <summary>
    /// 操作に応じてSetBomb()を呼び出す受け付ける
    /// </summary>
    virtual public void SetBombControl()
    {
    }

    /// <summary>
    /// 移動が完了した時に呼ばれるコールバック
    /// </summary>
    protected void MovedCallback()
    {
        // 操作対象を待機状態にする
        _player._action = Player.ACTION.STAY;
    }

    /// <summary>
    /// マップを知る
    /// </summary>
    public void SetMap(MapController map)
    {
        if(map == null)
        {
            Debug.Log("Null was set to PlayerController.");
        }

        _map = map;
    }

    /// <summary>
    /// 操作対象を知る
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(Player player)
    {
        Debug.Log("Player was set.");

        _player = player;
    }
}

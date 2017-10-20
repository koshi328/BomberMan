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

    // ステート
    public enum STATE
    {
        STAY,   // 待機
        MOVE,   // 移動中
        ACTION_NUM,
    }

    // ボムの最大所持数
    public static readonly int BOMB_MAX = 8;

    // スピードの最大レベル
    public static readonly int SPEED_MAX = 9;

    // 火力の最大レベル
    public static readonly int FIRE_MAX = 8;
    
    
    // マップ情報を取得するために持つ
    [SerializeField]
    protected MapController _map;
    
    // 何Pか？
    public int _playerNumber { get; set; }

    // 持てるボムの数
    public int _maxBombNum { get; set; }

    // ボムの今の所持数
    public int _currentBombNum { get; set; }
    
    // 火力レベル
    public int _fireLevel { get; set; }

    // スピードレベル
    public int _speedLevel { get; set; }

    // ステート
    public STATE _state { get; set; }

    // フラグ
    public int _flag = 0x00;
    public static readonly int SLOW = 0x01;
    public static readonly int QUICK = 0x02;
    public static readonly int INVINCIBLE = 0x04;
    public static readonly int KICKABLE = 0x08;
    public static readonly int CANSETMINE = 0x10;
    public static readonly int ISCONFUSION = 0x20;
    public static readonly int ISALIVE = 0x40;
    
    
    void Start ()
    {
        // 生存フラグを立てる
        _flag |= ISALIVE;

        // 待機状態にする
        _state = STATE.STAY;

        // ボムの最大所持数
        _maxBombNum = 1;

        // ボムの現在の所持数
        _currentBombNum = 1;

        // 火力レベル
        _fireLevel = 1;

        // スピードレベル
        _speedLevel = 1;
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
        _state = STATE.MOVE;
    }

    /// <summary>
    /// 移動が完了した時に呼ばれるコールバック
    /// </summary>
    protected void MovedCallback()
    {
        // 操作対象を待機状態にする
        _state = Player.STATE.STAY;

        // 移動したチップにアイテムがあれば取得する
        Item item = _map.GetItem(_position.x, _position.y);
        if (item == null) return;

        item.InfluenceEffect(this);
        item.gameObject.SetActive(false);
    }
    /// <summary>
    /// ボムを置く
    /// </summary>
    public void SetBomb()
    {
        // ボムが残っていなければおけない
        if(_currentBombNum < 1)
        {
            return;
        }

        _currentBombNum--;
        _map.SetBomb(_playerNumber, GetPosition().x, GetPosition().y, _fireLevel);
    }

    /// <summary>
    /// マップを知る
    /// </summary>
    public void SetMap(MapController map)
    {
        _map = map;
    }

    /// <summary>
    /// 死亡する
    /// </summary>
    public void Death()
    {
        // 消す
        gameObject.SetActive(false);

        // 生存フラグを折る
        SetStatus(ISALIVE, false);
    }

    /// <summary>
    /// フラグを変更する
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="flag"></param>
    public void SetStatus(int tag, bool flag)
    {
        if(flag)
        {
            _flag |= tag;
        }
        else
        {
            _flag &=~tag;
        }
    }

    /// <summary>
    /// フラグを取得する
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool GetStatus(int tag)
    {
        return (_flag & tag) != 0;
    }
}

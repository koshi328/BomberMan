using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MapObject
{
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
    public static readonly int SPEED_MAX = 8;

    // 火力の最大レベル
    public static readonly int FIRE_MAX = 8;

    // 混乱の時間
    public static readonly float DOKURO_TIME = 7.0f;

    // すり抜けの時間
    public static readonly float INVINCIBLE_TIME = 10.0f;

    // 気絶の時間
    public static readonly float STAN_TIME = 1.0f;

    // マップ情報を取得するために持つ
    [SerializeField]
    protected MapController _map;

    // 何Pか？
    public int _playerNumber;

    // 持てるボムの数
    public int _maxBombNum;

    // ボムの今の所持数
    public int _currentBombNum;

    // 火力レベル
    [SerializeField]
    public int _fireLevel;

    // スピードレベル
    [SerializeField]
    public int _speedLevel;

    // ステート
    public STATE _state { get; set; }

    // ドクロ用のタイマー
    private float _dokuroTimer;

    // すり抜け用のタイマー
    private float _invincibleTimer;

    // 気絶用のタイマー
    private float _stanTimer;

    // フラグ
    private int _flag = 0x00;
    public static readonly int SLOW = 0x01;
    public static readonly int QUICK = 0x02;
    public static readonly int INVINCIBLE = 0x04;
    public static readonly int KICKABLE = 0x08;
    public static readonly int ISCONFUSION = 0x10;
    public static readonly int ISALIVE = 0x20;
    public static readonly int ISSTAN = 0x40;

    public static readonly float MOVE_TIME_LEVEL_ONE = 0.4f;
    public static readonly float MOVE_TIME_INTERVAL = 0.04f;

    void Start()
    {
        // 生存フラグを立てる
        _flag |= ISALIVE;

        // 待機状態にする
        _state = STATE.STAY;

        // ボムの最大所持数
        _maxBombNum = 1;

        // ボムの現在の所持数
        _currentBombNum = _maxBombNum;

        // 火力レベル
        _fireLevel = 1;

        // スピードレベル
        _speedLevel = 1;

        // ドクロのタイマーの初期化
        _dokuroTimer = DOKURO_TIME;

        // すり抜けのタイマーの初期化
        _invincibleTimer = INVINCIBLE_TIME;
    }

    public void MyUpdate()
    {
        if (GetStatus(INVINCIBLE))
        {
            _invincibleTimer -= Time.deltaTime;

            if (_invincibleTimer < 0.0f)
            {
                SetStatus(INVINCIBLE, false);
                _invincibleTimer = INVINCIBLE_TIME;
            }
        }

        if (GetStatus(SLOW))
        {
            _dokuroTimer -= Time.deltaTime;

            if (_dokuroTimer < 0.0f)
            {
                SetStatus(SLOW, false);
                _dokuroTimer = DOKURO_TIME;
            }
        }
        else if (GetStatus(QUICK))
        {
            _dokuroTimer -= Time.deltaTime;

            if (_dokuroTimer < 0.0f)
            {
                SetStatus(QUICK, false);
                _dokuroTimer = DOKURO_TIME;
            }
        }
        else if (GetStatus(ISCONFUSION))
        {
            _dokuroTimer -= Time.deltaTime;

            if (_dokuroTimer < 0.0f)
            {
                SetStatus(ISCONFUSION, false);
                _dokuroTimer = DOKURO_TIME;
            }
        }

        if (GetStatus(ISSTAN))
        {
            _stanTimer -= Time.deltaTime;

            if (_stanTimer < 0.0f)
            {
                SetStatus(ISSTAN, false);
                _stanTimer = STAN_TIME;
            }
        }
    }


    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        // 気絶中なら処理しない
        if (GetStatus(ISSTAN)) return;

        // 現在地を取得
        MapController.Position currentPosition = GetPosition();
        MapController.Position destination = new MapController.Position(currentPosition.x, currentPosition.y);

        // 混乱状態なら移動方向を逆にする
        if (GetStatus(ISCONFUSION))
        {
            direction.x *= -1.0f;
            direction.y *= -1.0f;
        }

        // チップの情報を保存するための変数とりあえず破壊不可能ブロック
        MapController.STATE state = MapController.STATE.IMMUTABLE_BLOCK;

        // 目的地を計算
        destination.x += (int)(direction.x);
        destination.y += (int)(direction.y);

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
                // すり抜け状態ならボムをすり抜ける
                if(GetStatus(INVINCIBLE))
                {
                    // ボムキックをさせない
                    break;
                }
                // 目的地にボムがあって、キック可能ならキック！
                if (GetStatus(KICKABLE))
                {
                    _map.MoveBomb(destination.x, destination.y, direction);
                }
                return;
        }

        // チップが移動可能なマスなら移動
        Vector3 destinationPosition = MapController.GetChipPosition(destination.x, destination.y);
        float time = MOVE_TIME_LEVEL_ONE - MOVE_TIME_INTERVAL * (_speedLevel - 1);

        if(GetStatus(SLOW))
        {
            time = MOVE_TIME_LEVEL_ONE / 0.8f;
        }
        if (GetStatus(QUICK))
        {
            time = MOVE_TIME_LEVEL_ONE * 0.3f;
        }

        transform.DOMove(destinationPosition, time).OnComplete(() => {
            // アニメーションが終了時によばれる
            // 操作対象を待機状態にする
            _state = Player.STATE.STAY;

            SetPosition(destination.x, destination.y, true);

            // 移動したチップにアイテムがあれば取得する
            Item item = _map.GetItem(_position.x, _position.y);
            if (item == null) return;

            item.InfluenceEffect(this);
            item.gameObject.SetActive(false);

        });
        _state = STATE.MOVE;
    }

    /// <summary>
    /// ボムを置く
    /// </summary>
    public void SetBomb()
    {
        // ボムが残っていなければおけない
        if (_currentBombNum < 1)
        {
            return;
        }

        // ボムが置いてあるところにはおけない
        if(_map.GetChipState(_position.x, _position.y) == MapController.STATE.BOMB)
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
        if (flag)
        {
            _flag |= tag;
        }
        else
        {
            _flag &= ~tag;
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

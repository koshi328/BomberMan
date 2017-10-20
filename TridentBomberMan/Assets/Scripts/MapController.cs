﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // チップの情報
    public enum STATE
    {
        NONE,           // 地面
        IMMUTABLE_BLOCK,// 破壊不可能ブロック
        BREAKABLE_BLOCK,// 破壊可能ブロック
        BOMB            // 爆弾
    }

    // 座標
    public class Position
    {
        public Position(int a, int b)
        {
            x = a;
            y = b;
        }
        public int x;
        public int y;
    }

    // 幅
    static readonly int WIDTH = 15;

    // 高さ
    static readonly int HEIGHT = 13;

    // チップサイズ
    static readonly float CHIP_SIZE = 1.28f;

    // ボムの最大数
    static readonly int BOMB_LIMIT_NUM = 50;

    [SerializeField, Header("破壊不可能ブロックのプレハブ")]
    GameObject _immutableBlockPref;

    [SerializeField, Header("破壊可能ブロックのプレハブ")]
    GameObject _breakableBlockPref;

    [SerializeField, Header("ボムのプレハブ")]
    Bomb _bombPrefab;

    // シンプルステージのチップ情報
    readonly int[,] SIMPLE_STAGE =
    {
        {1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1},
        {1,0,0,2,2, 2,2,2,2,2, 2,2,0,0,1},
        {1,0,1,2,1, 2,1,2,1,2, 1,2,1,0,1},
        {1,0,2,2,2, 2,2,2,2,2, 2,2,2,2,1},
        {1,2,1,2,1, 2,1,2,1,2, 1,2,1,2,1},

        {1,2,2,2,2, 2,2,2,2,2, 2,2,2,2,1},
        {1,2,1,2,1, 2,1,2,1,2, 1,2,1,2,1},
        {1,2,2,2,2, 2,2,2,2,2, 2,2,2,2,1},
        {1,2,1,2,1, 2,1,2,1,2, 1,2,1,2,1},
        {1,2,2,2,2, 2,2,2,2,2, 2,2,2,2,1},

        {1,0,1,2,1, 2,1,2,1,2, 1,2,1,0,1},
        {1,0,0,2,2, 2,2,2,2,2, 2,2,0,0,1},
        {1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1},
    };

    // 現在のチップ情報
    int[,] _stage;

    // マップ上のブロックのインスタンス
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    // ボムのインスタンス
    Bomb[] _bomb;


    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void Init()
    {
        _stage = SIMPLE_STAGE;
        CreateObjects();
    }

    /// <summary>
    /// マップ上のブロック等のオブジェクト生成
    /// </summary>
    void CreateObjects()
    {
        // マップのブロックの初期化
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                float x = -(WIDTH - 1) * CHIP_SIZE / 2 + j * CHIP_SIZE;
                float y = -(HEIGHT - 1) * CHIP_SIZE / 2 + i * CHIP_SIZE;

                switch (_stage[i, j])
                {
                    case (int)STATE.IMMUTABLE_BLOCK:
                        _objects.Add(GetKey(j, i), Instantiate(_immutableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform));
                        break;
                    case (int)STATE.BREAKABLE_BLOCK:
                        _objects.Add(GetKey(j, i), Instantiate(_breakableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform));
                        break;
                }
            }
        }

        // ボムの初期化
        _bomb = new Bomb[BOMB_LIMIT_NUM];
        for (int i = 0; i < BOMB_LIMIT_NUM; i++)
        {
            _bomb[i] = Instantiate<Bomb>(_bombPrefab, Vector3.zero, Quaternion.identity, transform);
            _bomb[i].SetActive(false);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void MyUpdate()
    {
        // アクティブなボムの更新
        for (int i = 0; i < BOMB_LIMIT_NUM; i++)
        {
            if (_bomb[i].GetActive() == true)
            {
                _bomb[i].MyUpdate();

                if (_bomb[i]._currentElapsedTime < 0)
                {
                    Debug.Log("explode");
                    ExplodeBomb(_bomb[i]);
                }
            }
        }
    }


    /// <summary>
    /// ボムを置く
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fireLevel"></param>
    public void SetBomb(int x, int y, int fireLevel)
    {
        for (int i = 0; i < BOMB_LIMIT_NUM; i++)
        {
            // 使用中なら次のボムをチェック
            if (_bomb[i].GetActive() == true)
                continue;

            // 火力の設定
            _bomb[i]._fireLevel = fireLevel;

            // 座標の設定
            _bomb[i].Init();
            _bomb[i].SetPosition(x, y, false);
            _bomb[i].transform.position = GetChipPosition(x, y);
            SetChipState(x, y, STATE.BOMB);

            // 使用中にする
            _bomb[i].SetActive(true);

            return;
        }
    }

    /// <summary>
    /// ボムの爆発
    /// </summary>
    /// <param name="position"></param>
    /// <param name="fireLevel"></param>
    private void ExplodeBomb(Bomb bomb)
    {
        MapController.Position bombPosition = bomb.GetPosition();
        int fireLevel = bomb._fireLevel;
        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0),
        };

        for (int i = 0; i < dir.Length; i++)
        {
            for (int j = 1; j <= fireLevel; j++)
            {
                int x = (int)(bombPosition.x + (dir[i].x * j));
                int y = (int)(bombPosition.y + (dir[i].y * j));

                if (IsOutOfRange(x, y))
                    continue;

                switch ((STATE)_stage[y, x])
                {
                    case STATE.NONE:
                        break;
                    case STATE.IMMUTABLE_BLOCK:
                        j = fireLevel;
                        break;
                    case STATE.BREAKABLE_BLOCK:
                        SetChipState(x, y, STATE.NONE);
                        if (_objects[GetKey(x, y)] != null)
                        {
                            Destroy(_objects[GetKey(x, y)]);
                        }
                        j = fireLevel;
                        break;
                    case STATE.BOMB:
                        SetChipState(x, y, STATE.NONE);
                        break;
                }
            }
            SetChipState(bombPosition.x, bombPosition.y, STATE.NONE);
            bomb.SetActive(false);
        }
    }


    /// <summary>
    /// 配列の外か？
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsOutOfRange(int x, int y)
    {
        if (x < 1) return true;
        if (WIDTH - 1 < x) return true;
        if (y < 1) return true;
        if (HEIGHT - 1 < y) return true;

        return false;
    }

    /// <summary>
    /// 指定されたチップの情報を返す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public MapController.STATE GetChipState(int x, int y)
    {
        return (STATE)_stage[y, x];
    }

    /// <summary>
    /// 指定されたチップの情報を書き換える
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="state"></param>
    public void SetChipState(int x, int y, MapController.STATE state)
    {
        if (IsOutOfRange(x, y)) return;

        _stage[y, x] = (int)state;
    }

    /// <summary>
    /// 指定されたチップの座標を返す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static public Vector3 GetChipPosition(int x, int y)
    {
        if (x < 1) return Vector3.zero;
        if (WIDTH - 1 < x) return Vector3.zero;
        if (y < 1) return Vector3.zero;
        if (HEIGHT - 1 < y) return Vector3.zero;

        Vector3 pos = Vector3.zero;
        pos.x = -(WIDTH - 1) * CHIP_SIZE / 2 + x * CHIP_SIZE;
        pos.y = -(HEIGHT - 1) * CHIP_SIZE / 2 + y * CHIP_SIZE;

        return pos;
    }

    static public int GetKey(int x, int y)
    {
        return x * 100 + y;
    }
}

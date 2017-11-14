﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapController : MonoBehaviour
{
    // チップの情報
    public enum STATE
    {
        NONE,           // 地面
        IMMUTABLE_BLOCK,// 破壊不可能ブロック
        BREAKABLE_BLOCK,// 破壊可能ブロック
        BOMB,           // 爆弾
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
    public static readonly int WIDTH = 15;

    // 高さ
    public static readonly int HEIGHT = 13;

    // チップサイズ
    static readonly float CHIP_SIZE = 1.28f;

    // ボムの最大数
    static readonly int BOMB_LIMIT_NUM = 50;

    // アイテムの最大数
    static readonly int ITEM_LIMIT_NUM = 50;

    // アイテムの入ってる率
    static readonly float ITEM_RATE = 0.7f;

    // 爆発エフェクトの最大数
    static readonly int EXPLOSION_LIMIT_NUM = 1000;

    [SerializeField, Header("破壊不可能ブロックのプレハブ")]
    GameObject _immutableBlockPref;

    [SerializeField, Header("破壊可能ブロックのプレハブ")]
    GameObject _breakableBlockPref;

    [SerializeField, Header("ボムのプレハブ")]
    Bomb _bombPrefab;

    [SerializeField]
    GameObject[] _itemPrefab = new GameObject[(int)Item.KIND.KIND_NUM];

    // 爆発エフェクトのプレハブ
    [SerializeField]
    Explosion _explosionPrefab;

    // メインカメラ
    [SerializeField]
    Camera _camera;

    // シンプルステージのチップ情報
    readonly int[,] SIMPLE_STAGE =
    {
        {1,1,1,1,1, 1,1,1,1,1, 1,1,1,1,1},
        {1,0,0,2,2, 2,2,2,2,2, 2,2,0,0,1},
        {1,0,1,2,1, 2,1,2,1,2, 1,2,1,0,1},
        {1,2,2,2,2, 2,2,2,2,2, 2,2,2,2,1},
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
    Dictionary<int, Block> _block = new Dictionary<int, Block>();

    // アイテム
    Item[] _item;

    // ボムのインスタンス
    Bomb[] _bomb = new Bomb[BOMB_LIMIT_NUM];

    // 爆発エフェクト
    Explosion[] _explosion = new Explosion[EXPLOSION_LIMIT_NUM];

    [SerializeField]
    BattleManager _battleManager;

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
        List<Block> array = new List<Block>();

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
                        _block.Add(GetKey(j, i), Instantiate(_immutableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform).GetComponent<Block>());
                        break;
                    case (int)STATE.BREAKABLE_BLOCK:
                        _block.Add(GetKey(j, i), Instantiate(_breakableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform).GetComponent<Block>());
                        _block[GetKey(j, i)].SetPosition(j, i, false);
                        array.Add(_block[GetKey(j, i)]);
                        break;
                }
            }
        }

        // アイテムの初期化
        for (int i = 0; i < array.Count; i++)
        {
            int elem = Random.Range(0, array.Count);
            Block temp = array[i];
            array[i] = array[elem];
            array[elem] = temp;
        }
        int n = (int)(array.Count * ITEM_RATE);
        _item = new Item[n];
        for (int i = 0; i < n; i++)
        {
            int m = Random.Range(0, 1000);

            if(0 <= m && m < 300)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.BOMB_UP], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (300 <= m && m < 600)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.FIRE_UP], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (600 <= m && m < 900)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.SPEED_UP], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (900 <= m && m < 925)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.FIRE_FULL], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (925 <= m && m < 950)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.DOKURO], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (950 <= m && m < 975)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.BOMB_KICK], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }
            if (975 <= m)
            {
                _item[i] = Instantiate(_itemPrefab[(int)Item.KIND.INVINCIBLE], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            }

            _item[i].SetPosition(array[i].GetPosition().x, array[i].GetPosition().y, false);
            array[i].SetItem(_item[i]);

            //int m = Random.Range(0, (int)Item.KIND.KIND_NUM);
            //_item[i] = Instantiate(_itemPrefab[m], Vector3.zero, Quaternion.identity, transform).GetComponent<Item>();
            //_item[i].SetPosition(array[i].GetPosition().x, array[i].GetPosition().y, false);
            //array[i].SetItem(_item[i]);
        }

        array.Clear();

        // ボムの初期化
        for (int i = 0; i < BOMB_LIMIT_NUM; i++)
        {
            _bomb[i] = Instantiate<Bomb>(_bombPrefab, Vector3.zero, Quaternion.identity, transform);
            _bomb[i].SetActive(false);
        }

        // エフェクトの初期化
        for (int i = 0; i < EXPLOSION_LIMIT_NUM; i++)
        {
            _explosion[i] = Instantiate<Explosion>(_explosionPrefab, Vector3.zero, Quaternion.identity, transform);
            _explosion[i].gameObject.SetActive(false);
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
                    ExplodeBomb(_bomb[i]);
                }
            }
        }
    }


    /// <summary>
    /// ボムを置く
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fireLevel"></param>
    public void SetBomb(int playerNumber, int x, int y, int fireLevel)
    {
        AudioController.Play("BombSet");

        for (int i = 0; i < BOMB_LIMIT_NUM; i++)
        {
            // 使用中なら次のボムをチェック
            if (_bomb[i].GetActive() == true)
                continue;

            // 火力の設定
            _bomb[i]._fireLevel = fireLevel;

            // 座標の設定
            _bomb[i].Init(playerNumber, x, y, fireLevel);
            _bomb[i].SetPosition(x, y, false);
            _bomb[i].transform.position = GetChipPosition(x, y);
            SetChipState(x, y, STATE.BOMB);

            // 使用中にする
            _bomb[i].SetActive(true);

            return;
        }
    }

    /// <summary>
    /// ボムの移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="direction"></param>
    public void MoveBomb(int x, int y, Vector2 direction)
    {
        // 指定された爆弾を探す
        for (int i = 0; i < _bomb.Length; i++)
        {
            if (_bomb[i].GetPosition().x == x && _bomb[i].GetPosition().y == y)
            {
                _bomb[i].Move(direction, this, _battleManager);
            }
        }
    }

    /// <summary>
    /// ボムの爆発
    /// </summary>
    /// <param name="position"></param>
    /// <param name="fireLevel"></param>
    private void ExplodeBomb(Bomb bomb)
    {
        AudioController.Play("BombExplode");

        // ボムの所持数を回復
        _battleManager.GetPlayer(bomb._playerNumber)._currentBombNum++;

        // ボムの座標を取得
        MapController.Position bombPosition = bomb.GetPosition();
        int fireLevel = bomb._fireLevel;

        // 火力が一定以上なら画面を揺らす
        //if(fireLevel)
        _camera.transform.DOShakePosition(0.1f);
        _camera.transform.DOMove(new Vector3(0.0f, 0.0f, -10.0f), 0.1f);

        Vector2[] dir =
        {
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2(-1, 0),
        };

        for (int i = 0; i < dir.Length; i++)
        {
            for (int j = 0; j < fireLevel + 1; j++)
            {
                int x = (int)(bombPosition.x + (dir[i].x * j));
                int y = (int)(bombPosition.y + (dir[i].y * j));

                if (IsOutOfRange(x, y))
                    continue;

                switch ((STATE)_stage[y, x])
                {
                    case STATE.NONE:
                        // 爆発エフェクトの再生
                        PlayExplosionEffect(x, y);

                        // プレイヤーとの当たり判定
                        if (!_battleManager._isFinished)
                        {
                            for (int k = 0; k < _battleManager._playerNum; k++)
                            {
                                if (_battleManager.GetPlayer(k).GetPosition().x == x && _battleManager.GetPlayer(k).GetPosition().y == y)
                                {
                                    _battleManager.GetPlayer(k).Death();
                                }
                            }
                        }

                        // アイテムとの当たり判定
                        for (int k = 0; k < _item.Length; k++)
                        {
                            if (_item[k].gameObject.GetActive() == false) continue;

                            if (_item[k].GetPosition().x == x && _item[k].GetPosition().y == y)
                            {
                                _item[k].gameObject.SetActive(false);
                            }
                        }
                        break;
                    case STATE.IMMUTABLE_BLOCK:
                        j = fireLevel + 1;
                        break;
                    case STATE.BREAKABLE_BLOCK:
                        // 爆発エフェクトの再生
                        PlayExplosionEffect(x, y);
                        SetChipState(x, y, STATE.NONE);
                        // 破壊可能ブロックとの当たり判定
                        if (_block[GetKey(x, y)] != null)
                        {
                            Destroy(_block[GetKey(x, y)].gameObject);
                        }
                        j = fireLevel + 1;
                        break;
                    case STATE.BOMB:
                        // 爆発エフェクトの再生
                        PlayExplosionEffect(x, y);
                        SetChipState(x, y, STATE.NONE);
                        // プレイヤーとの当たり判定
                        if (!_battleManager._isFinished)
                        {
                            for (int k = 0; k < _battleManager._playerNum; k++)
                            {
                                if (_battleManager.GetPlayer(k).GetPosition().x == x && _battleManager.GetPlayer(k).GetPosition().y == y)
                                {
                                    _battleManager.GetPlayer(k).Death();
                                }
                            }
                        }
                        // 他の爆弾との当たり判定
                        for (int k = 0; k < _bomb.Length; k++)
                        {
                            if (_bomb[k].GetPosition().x == x && _bomb[k].GetPosition().y == y)
                            {
                                _bomb[k].Detonate();
                            }
                        }
                        break;
                }
            }
            SetChipState(bombPosition.x, bombPosition.y, STATE.NONE);
            bomb.SetActive(false);
        }
    }

    /// <summary>
    /// 爆発エフェクトの再生
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void PlayExplosionEffect(int x, int y)
    {
        for (int i = 0; i < EXPLOSION_LIMIT_NUM; i++)
        {
            // 使用中なら次のエフェクトをチェック
            if (_explosion[i].gameObject.GetActive() == true)
                continue;

            // 座標の設定
            _explosion[i].transform.position = GetChipPosition(x, y);
            _explosion[i].Play();

            return;
        }
    }

    /// <summary>
    /// プレイヤーがアイテムを取得する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Item GetItem(int x, int y)
    {
        for (int i = 0; i < _item.Length; i++)
        {
            if (_item[i].gameObject.GetActive() == false) continue;

            if (x == _item[i].GetPosition().x &&
               y == _item[i].GetPosition().y)
            {
                return _item[i];
            }
        }

        return null;
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
        if (IsOutOfRange(x, y)) return STATE.IMMUTABLE_BLOCK;
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
        //if (x < 0) return Vector3.zero;
        //if (WIDTH - 1 < x) return Vector3.zero;
        //if (y < 0) return Vector3.zero;
        //if (HEIGHT - 1 < y) return Vector3.zero;

        Vector3 pos = Vector3.zero;
        pos.x = -(WIDTH - 1) * CHIP_SIZE / 2 + x * CHIP_SIZE;
        pos.y = -(HEIGHT - 1) * CHIP_SIZE / 2 + y * CHIP_SIZE;

        return pos;
    }

    static public int GetKey(int x, int y)
    {
        return x * 100 + y;
    }

    public int GetWidth()
    {
        return WIDTH;
    }
    public int GetHeight()
    {
        return HEIGHT;
    }
    public Bomb[] GetBomb()
    {
        return _bomb;
    }
    public Item[] GetItems()
    {
        return _item;
    }
}

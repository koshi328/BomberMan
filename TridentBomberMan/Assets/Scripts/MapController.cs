using System.Collections;
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
    public struct Position
    {
        public int x;
        public int y;
    }

    // 幅
    static readonly int WIDTH = 15;

    // 高さ
    static readonly int HEIGHT = 13;

    // チップサイズ
    static readonly float CHIP_SIZE = 1.28f;

    [SerializeField, Header("破壊不可能ブロック")]
    GameObject _immutableBlockPref;

    [SerializeField, Header("破壊可能ブロック")]
    GameObject _breakableBlockPref;

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



    /// <summary>
    /// 初期化
    /// </summary>
    void Start ()
    {
        Init();
        CreateObjects();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void Init()
    {
        _stage = SIMPLE_STAGE;
    }

    /// <summary>
    /// マップ上のブロック等のオブジェクト生成
    /// </summary>
    void CreateObjects()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                float x = -(WIDTH - 1) * CHIP_SIZE / 2 + j * CHIP_SIZE;
                float y = -(HEIGHT - 1) * CHIP_SIZE / 2 + i * CHIP_SIZE;

                switch (_stage[i, j])
                {
                    case (int)STATE.IMMUTABLE_BLOCK:
                        Instantiate(_immutableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                        break;
                    case (int)STATE.BREAKABLE_BLOCK:
                        Instantiate(_breakableBlockPref, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                        break;
                }
            }
        }
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
        if (x < 1) return;
        if (WIDTH - 1 < x) return;
        if (y < 1) return;
        if (HEIGHT - 1 < y) return;

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
}

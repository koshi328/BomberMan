using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
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
    readonly int WIDTH = 15;

    // 高さ
    readonly int HEIGHT = 13;

    // チップサイズ
    readonly float CHIP_SIZE = 1.28f;

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
    /// 初期化
    /// </summary>
    public void Init()
    {
        _stage = SIMPLE_STAGE;
    }

    /// <summary>
    /// オブジェクト生成
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

    public MapController.STATE GetChipState(int x, int y)
    {
        if (x < 1) return STATE.IMMUTABLE_BLOCK;
        if (WIDTH - 1 < x) return STATE.IMMUTABLE_BLOCK;
        if (y < 1) return STATE.IMMUTABLE_BLOCK;
        if (HEIGHT - 1 < y) return STATE.IMMUTABLE_BLOCK;

        return (STATE)_stage[y, x];
    }
}

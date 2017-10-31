using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    // ノードの状態
    enum STATE
    {
        NONE,
        OPEN,
        CLOSE,
    }

    // 現在のステージ情報
    int[,] _stage;

    // 経路
    MapController.Position[] _path;



    /// <summary>
    /// ステージの情報を更新する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="state"></param>
    public void UpdateStageInfo(int x, int y, MapController.STATE state)
    {
        _stage[y, x] = (int)state;
    }
}

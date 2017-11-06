using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfuluenceMap : MonoBehaviour{

    [SerializeField]
    MapController _map;

    // プレイヤーからの距離をダイクストラ法で格納したマップ
    int[,] _dijkstraMap;

    // 影響マップ
    float[,] _infuluenceMap;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // マップ上での座標
    protected MapController.Position _position;



    /// <summary>
    /// 初期化
    /// </summary>
    void Awake ()
    {
        _position = new MapController.Position(0, 0);
	}

    /// <summary>
    /// 座標を変更する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tweening">DOMove()等の処理中か？</param>
    public void SetPosition(int x, int y, bool tweening)
    {
        _position.x = x;
        _position.y = y;

        if (tweening) return;

        transform.position = MapController.GetChipPosition(x, y);
    }

    /// <summary>
    /// マップ上の座標を返す
    /// </summary>
    /// <returns></returns>
    public MapController.Position GetPosition()
    {
        return _position;
    }
}

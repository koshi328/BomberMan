using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // マップ上での座標
    protected MapController.Position _position;

    void Start ()
    {

	}

    public void SetPosition(MapController.Position position)
    {
        _position = position;
    }

    public MapController.Position GetPosition()
    {
        return _position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    // 移動方向
    public enum DIRECTION
    {
        NONE,
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }

    // マップ情報を取得するために持つ
    MapController _map;

    // 操作する対象
    Player _player;

	void Start()
    {

	}
	
	void Update()
    {
		
	}

    public void Move(DIRECTION direction)
    {
        // 現在地を取得
        MapController.Position currentPosition = _player.GetPosition();
        MapController.Position destination;
        destination.x = 0;
        destination.y = 0;

        MapController.STATE state = MapController.STATE.IMMUTABLE_BLOCK;

        // 移動方向
        switch (direction)
        {
            case DIRECTION.UP:
                destination.y = -1;
                break;
            case DIRECTION.DOWN:
                destination.y = 1;
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

        switch (state)
        {
            case MapController.STATE.NONE:

                break;
            case MapController.STATE.IMMUTABLE_BLOCK:

                break;
            case MapController.STATE.BREAKABLE_BLOCK:

                break;
            case MapController.STATE.BOMB:

                break;
        }
    }
}

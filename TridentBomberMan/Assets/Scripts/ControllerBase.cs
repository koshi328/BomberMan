using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase
{
    protected MapController _map;

    // 操作する対象
    [SerializeField]
    protected Player _player;

	void Start()
    {

	}
	
	public virtual void MyUpdate()
    {
        //if (_player.GetStatus(Player.ISALIVE) == false) return;

        _player.MyUpdate();
        ControlMove();
        ControlSetBomb();
    }

    

    /// <summary>
    /// 操作に応じてMove()を呼び出す仮想関数
    /// </summary>
    virtual public void ControlMove()
    {
    }

    /// <summary>
    /// 操作に応じてSetBomb()を呼び出す受け付ける
    /// </summary>
    virtual public void ControlSetBomb()
    {
    }

    /// <summary>
    /// 操作対象を知る
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(Player player)
    {
        _player = player;
    }

    /// <summary>
    /// マップを知る
    /// </summary>
    /// <param name="map"></param>
    public void SetMap(MapController map)
    {
        _map = map;
    }
}

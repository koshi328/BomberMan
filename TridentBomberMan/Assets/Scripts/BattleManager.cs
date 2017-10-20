using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // マップ
    [SerializeField]
    private MapController _map;

    // プレイヤーコントローラ
    private ControllerBase[] _controllers;

    // プレイヤーのプレハブ
    [SerializeField]
    private GameObject _playerPrefab;

    // プレイヤーの情報
    private Player[] _playerInfo;

    // プレイヤーのインスタンス
    private GameObject[] _playerInstances;

    // プレイヤーキャラクターの数
    [SerializeField]
    private int _playerNum;

    // 人間の数
    [SerializeField]
    private int _humanNum;



    void Awake ()
    {
        Init();
    }

    void Init()
    {
        // 設定シーンで設定した値を入れられるように後でする予定
        _playerNum = 1;
        _humanNum = 1;

        // プレイヤーのインスタンスを生成
        _playerInstances = new GameObject[_playerNum];

        // プレイヤーの情報を保存する配列を生成
        _playerInfo = new Player[_playerNum];

        // プレイヤーを操作するコントローラを生成
        _controllers = new ControllerBase[_playerNum];



        // インスタンスの生成と配置
        for (int i = 0; i < _playerNum; i++)
        {
            _playerInstances[i] = Instantiate(_playerPrefab);
        }

        // インスタンスから情報を取り出す
        for (int i = 0; i < _playerNum; i++)
        {
            // 初期化だけだからGetComponent()許して
            _playerInfo[i] = _playerInstances[i].GetComponent<Player>();

            switch (i)
            {
                case 0:
                    _playerInfo[i].SetMap(_map);
                    _playerInfo[i].SetPosition(1, 1, false);
                    break;
            }
        }

        // プレイヤーコントローラの設定
        for (int i = 0; i < _playerNum; i++)
        {
            if (i < _humanNum)
            {
                _controllers[i] = new HumanController();
            }
            else
            {
                _controllers[i] = new AutomaticController();
            }

            _controllers[i].SetPlayer(_playerInfo[i]);
        }
    }
	
	void Update ()
    {
        _map.MyUpdate();

        for (int i = 0; i < _playerNum; i++)
        {
            _controllers[i].MyUpdate();
        }
    }
}

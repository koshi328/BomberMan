using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // マップ
    [SerializeField]
    private MapController _map;
    MapNavigation _mapNavigation;

    // プレイヤーコントローラ
    private ControllerBase[] _controllers;

    // プレイヤーのプレハブ
    [SerializeField]
    private GameObject[] _playerPrefabs;

    // プレイヤーの情報
    private Player[] _playerInfo;

    // プレイヤーのインスタンス
    private GameObject[] _playerInstances;

    // プレイヤーキャラクターの数
    [SerializeField]
    public int _playerNum
    {
        get;
        set;
    }

    // 人間の数
    [SerializeField]
    private int _humanNum;

    // 対戦が終了しているか？
    public bool _isFinished = false;
    
    // 残り時間
    [SerializeField]
    private float _time;

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private GameObject _finishSprite;

    void Awake ()
    {
        DOTween.Init();    // ← コレないと効かない
        DOTween.defaultEaseType = Ease.InOutQuad;

        Init();
    }

    void Init()
    {
        // 設定シーンで設定した値を入れられるように後でする予定
        _playerNum = 4;
        _humanNum = 1;

        // マップを生成
        _map.Init();

        // プレイヤーのインスタンスを生成
        _playerInstances = new GameObject[_playerNum];

        // プレイヤーの情報を保存する配列を生成
        _playerInfo = new Player[_playerNum];

        // プレイヤーを操作するコントローラを生成
        _controllers = new ControllerBase[_playerNum];

        _isFinished = false;

        // 制限時間の表示の配置
        _timeText.transform.position = new Vector2(100.0f, 100.0f);

        // 終了を知らせる画像
        _finishSprite.transform.position = Vector3.zero;
        _finishSprite.transform.localScale = Vector3.zero;
        _finishSprite.gameObject.SetActive(false);

        // インスタンスの生成と配置
        for (int i = 0; i < _playerNum; i++)
        {
            _playerInstances[i] = Instantiate(_playerPrefabs[i]);
        }

        // インスタンスから情報を取り出す
        for (int i = 0; i < _playerNum; i++)
        {
            // 初期化だけだからGetComponent()許して
            _playerInfo[i] = _playerInstances[i].GetComponent<Player>();

            _playerInfo[i]._playerNumber = i;

            switch (i)
            {
                case 0:
                    _playerInfo[i].SetMap(_map);
                    _playerInfo[i].SetPosition(1, 1, false);
                    break;
                case 1:
                    _playerInfo[i].SetMap(_map);
                    _playerInfo[i].SetPosition(13, 11, false);
                    break;
                case 2:
                    _playerInfo[i].SetMap(_map);
                    _playerInfo[i].SetPosition(1, 11, false);
                    break;
                case 3:
                    _playerInfo[i].SetMap(_map);
                    _playerInfo[i].SetPosition(13, 1, false);
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
            _controllers[i].SetMap(_map);
            _controllers[i].Initialize();
        }

        _mapNavigation = GameObject.Find("MapNavigation").GetComponent<MapNavigation>();
    }
	
	void Update ()
    {
        // 制限時間の更新
        if (!_isFinished)
        {
            _time -= Time.deltaTime;
            int time = (int)_time;
            _timeText.text = time.ToString();

            if (_time < 0.0f)
            {
                _time = 0.0f;
                _finishSprite.gameObject.SetActive(true);
                _finishSprite.transform.DOScale(2.0f, 2.0f).SetEase(Ease.OutBounce);
                _isFinished = true;
            }

            int alivePlayerNum = 0;

            // プレイヤーが1人以下なら
            for(int i = 0;i< _playerNum;i++)
            {
                if(_playerInfo[i].GetStatus(Player.ISALIVE))
                {
                    alivePlayerNum++;
                }
            }

            if(alivePlayerNum <= 1)
            {
                _finishSprite.gameObject.SetActive(true);
                _finishSprite.transform.DOScale(2.0f, 2.0f).SetEase(Ease.OutBounce);
                _isFinished = true;
            }
        }

        // マップ情報の更新
        _map.MyUpdate();

        _mapNavigation.MyUpdate();
        // 対戦中のみプレイヤーの操作を受け付ける
        if (!_isFinished)
        {
            for (int i = 0; i < _playerNum; i++)
            {
                _controllers[i].MyUpdate();
            }
        }
    }

    /// <summary>
    /// プレイヤーの情報を返す
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public Player GetPlayer(int n)
    {
        if (n < 0) return null;
        if (_playerNum < n) return null;

        return _playerInfo[n];
    }
}

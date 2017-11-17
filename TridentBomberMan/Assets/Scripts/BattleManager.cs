using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static readonly float BACKBUTTON_X = -4.0f;
    public static readonly float BACKBUTTON_Y = -4.9f;
    public static readonly float REPLAYBUTTON_X = 4.0f;
    public static readonly float REPLAYBUTTON_Y = -4.9f;

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
    public int _playerNum;

    // 人間の数
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

    [SerializeField]
    private GameObject[] _winPlayerSprite;

    [SerializeField]
    private GameObject[] _selectButton;

    [SerializeField]
    private GameObject _selectingFrame;

    private int _selectingNumber;

    private bool _selectIsMoving;

    private float _selectCount;

    private bool _isReady;

    [SerializeField]
    private GameObject _readyBg;

    [SerializeField]
    private GameObject[] _readySprite;

    private int _countDown;

    private float _selectWait;


    void Awake ()
    {
        DOTween.Init();    // ← コレないと効かない
        DOTween.defaultEaseType = Ease.InOutQuad;

        Init();

        AudioController.PlayMusic("GameBgm");

        var go = GameObject.Find("FadeManager");
        if (!go) return;
        FadeManager fadeManager = go.GetComponent<FadeManager>();
        fadeManager.FadeIn();
    }

    void Init()
    {
        // 設定シーンで設定した値を入れられるように後でする予定
        var go = GameObject.Find("GlobalData");
        if (!go) return;
        GlobalData globalData = go.GetComponent<GlobalData>();
        _playerNum = globalData._playerNum;
        _humanNum = globalData._humanNum;

        // マップを生成
        _map.Init();

        // プレイヤーのインスタンスを生成
        _playerInstances = new GameObject[_playerNum];

        // プレイヤーの情報を保存する配列を生成
        _playerInfo = new Player[_playerNum];

        // プレイヤーを操作するコントローラを生成
        _controllers = new ControllerBase[_playerNum];

        _isFinished = false;

        _isReady = true;

        _countDown = 0;

        // 終了を知らせる画像
        _finishSprite.transform.localScale = Vector3.zero;
        _finishSprite.gameObject.SetActive(false);

        // 勝利プレイヤーの画像
        for (int i = 0; i < 4; i++)
        {
            _winPlayerSprite[i].transform.localScale = Vector3.zero;
            _winPlayerSprite[i].SetActive(false);
        }

        // ボタン
        _selectButton[0].transform.localPosition = new Vector3(BACKBUTTON_X, BACKBUTTON_Y - 10.0f, 0.0f);
        _selectButton[0].SetActive(false);
        _selectButton[1].transform.localPosition = new Vector3(REPLAYBUTTON_X, REPLAYBUTTON_Y -10.0f, 0.0f);
        _selectButton[1].SetActive(false);
        _selectingFrame.transform.localPosition = new Vector3(REPLAYBUTTON_X, REPLAYBUTTON_Y -10.0f, 0.0f);
        _selectingFrame.SetActive(false);

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

        _selectingNumber = 0;

        _selectIsMoving = false;

        _selectCount = 0.5f;

        _readySprite[0].SetActive(true);

        // カウントダウン
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_readySprite[0].transform.DOScale(0.0f, 1.0f).OnComplete(() => {
            _readySprite[0].SetActive(false);
            _readySprite[1].SetActive(true);
            Debug.Log(2);
        }));

        sequence.Append(_readySprite[1].transform.DOScale(0.0f, 1.0f).OnComplete(() => {
             _readySprite[1].SetActive(false);
             _readySprite[2].SetActive(true);
            Debug.Log(1);
         }));

        sequence.Append(_readySprite[2].transform.DOScale(0.0f, 1.0f).OnComplete(() => {
            _readySprite[2].SetActive(false);
            _readySprite[3].SetActive(true);
            Debug.Log("go");
        }));

        sequence.Append(_readySprite[3].transform.DOScale(0.0f, 1.0f).OnComplete(() => {
            _readySprite[3].SetActive(false);
            _readyBg.SetActive(false);
            _isReady = false;
        }));

        sequence.Play();

        _selectWait = 2.0f;
    }
	
	void Update ()
    {
        if (_isReady) return;
        // 制限時間の更新
        if (!_isFinished)
        {
            _time -= Time.deltaTime;
            int time = (int)_time;
            _timeText.text = time.ToString();

            if (_time < 0.0f)
            {
                _time = 0.0f;
                _selectButton[0].SetActive(true);
                _selectButton[0].transform.DOMoveY(BACKBUTTON_Y, 0.5f);
                _selectButton[1].SetActive(true);
                _selectButton[1].transform.DOMoveY(REPLAYBUTTON_Y, 0.5f);
                _selectingFrame.SetActive(true);
                _selectingFrame.transform.DOMoveY(REPLAYBUTTON_Y, 0.5f);
                _isFinished = true;
                AudioController.Play("Finish");
            }

            // 生存プレイヤーの人数と生存プレイヤーの番号
            int alivePlayerNum = 0;
            int alivePlayer = 0;

            // プレイヤーが1人以下なら
            for(int i = 0;i< _playerNum;i++)
            {
                if(_playerInfo[i].GetStatus(Player.ISALIVE))
                {
                    alivePlayerNum++;
                    alivePlayer = i;
                }
            }

            if(alivePlayerNum == 1)
            {
                _finishSprite.gameObject.SetActive(true);
                _finishSprite.transform.DOScale(2.0f, 2.0f).SetEase(Ease.OutBounce);
                _winPlayerSprite[alivePlayer].SetActive(true);
                _winPlayerSprite[alivePlayer].transform.DOScale(1.5f, 1.5f).SetEase(Ease.OutBounce);
                _selectButton[0].SetActive(true);
                _selectButton[0].transform.DOMoveY(BACKBUTTON_Y, 0.5f);
                _selectButton[1].SetActive(true);
                _selectButton[1].transform.DOMoveY(REPLAYBUTTON_Y, 0.5f);
                _selectingFrame.SetActive(true);
                _isFinished = true;
            }
            else if(alivePlayerNum < 1)
            {
                _selectButton[0].SetActive(true);
                _selectButton[0].transform.DOMoveY(BACKBUTTON_Y, 0.5f);
                _selectButton[1].SetActive(true);
                _selectButton[1].transform.DOMoveY(REPLAYBUTTON_Y, 0.5f);
                _selectingFrame.SetActive(true);
                _selectingFrame.transform.DOMoveY(REPLAYBUTTON_Y, 0.5f);
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

        if (float.Epsilon < GamepadInput.GamePad.GetTrigger(GamepadInput.GamePad.Trigger.LeftTrigger, GamepadInput.GamePad.Index.One))
        {
            SceneManager.LoadScene("Game");
        }

        UseButton();
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

    /// <summary>
    /// ボタンを選択したり押したりする
    /// </summary>
    private void UseButton()
    {
        if (!_isFinished) return;

        if (0.0f < _selectWait)
        {
            _selectWait -= Time.deltaTime;
            return;
        }

        // 選択中なら待つ
        if (_selectIsMoving)
        {
            _selectCount -= Time.deltaTime;

            if (_selectCount < 0.0f)
            {
                _selectIsMoving = false;
            }
        }
        else
        {
            Vector2 axis = GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, GamepadInput.GamePad.Index.One);

            // 左右で選択
            if (0.9f < Mathf.Abs(axis.x) ||
                Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.LeftArrow))
            {
                AudioController.Play("Select");
                _selectIsMoving = true;
                _selectCount = 0.1f;

                _selectingNumber = 1 - _selectingNumber;

                if (_selectingNumber == 0)
                {
                    _selectingFrame.transform.localPosition = new Vector3(BACKBUTTON_X, BACKBUTTON_Y, 0.0f);
                }
                else
                {
                    _selectingFrame.transform.localPosition = new Vector3(REPLAYBUTTON_X, REPLAYBUTTON_Y, 0.0f);
                }
            }
        }

        // 決定する
        if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.X, GamepadInput.GamePad.Index.One) ||
        Input.GetKeyDown(KeyCode.Space))
        {
            AudioController.Play("Decide");
            switch (_selectingNumber)
            {
                case 0:
                    SceneManager.LoadScene("Edit", LoadSceneMode.Single);
                    break;
                case 1:
                    SceneManager.LoadScene("Game", LoadSceneMode.Single);
                    break;
            }
        }
    }
}

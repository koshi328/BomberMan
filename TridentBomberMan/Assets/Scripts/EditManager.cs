using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour {
    [SerializeField]
    Button[] _buttonList;
    [SerializeField]
    Image _cursor;
    [SerializeField]
    Sprite[] _stateImage;

    int[] _playerState;
    int _selectNum;

    public static readonly float INPUT_INTERVAL = 0.1f;
    float _intervalTime;

    // Use this for initialization
    void Awake () {
        _selectNum = 0;
        SetButton(_selectNum);
        _playerState = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _playerState[i] = i;
        }

        _intervalTime = 0.0f;

        AudioController.PlayMusic("EditBgm");
    }
	
	// Update is called once per frame
	void Update () {

        if (_intervalTime < INPUT_INTERVAL)
        {
            _intervalTime += Time.deltaTime;
        }
        else
        {
            _intervalTime = 0.0f;

            // 取得するキー情報のタグ
            var leftStickHorizontal = "L_XAxis_1";
            var leftStickVertical = "L_YAxis_1";
            var dpadHorizontal = "DPad_XAxis_1";
            var dpadVertical = "DPad_YAxis_1";

            // 入力を取得
            float xInput = Input.GetAxisRaw(leftStickHorizontal);
            float yInput = Input.GetAxisRaw(leftStickVertical);
            if (Mathf.Abs(xInput) < float.Epsilon &&
                Mathf.Abs(yInput) < float.Epsilon)
            {
                xInput = Input.GetAxisRaw(dpadHorizontal);
                yInput = Input.GetAxisRaw(dpadVertical);
            }

            // 縦方向の移動の入力がされていたら
            if (float.Epsilon < Mathf.Abs(yInput))
            {
                AudioController.Play("Select");

                if (yInput < 0)
                {
                    if (--_selectNum < 0) _selectNum = _buttonList.Length - 1;
                    SetButton(_selectNum);
                }
                else
                {
                    if (++_selectNum >= _buttonList.Length) _selectNum = 0;
                    SetButton(_selectNum);
                }
            }
        }
    }

    public void ChangeScene(string name)
    {
        AudioController.Play("Decide");

        var go = GameObject.Find("FadeManager");
        if (!go) return;
        FadeManager fadeManager = go.GetComponent<FadeManager>();

        if(name == "Title")
        {
            fadeManager.ChangeScene(0);
        }
        else if(name == "Edit")
        {
            fadeManager.ChangeScene(1);
        }
        else
        {
            fadeManager.ChangeScene(2);

            var go2 = GameObject.Find("GlobalData");
            if (!go2) return;
            GlobalData globalData = go2.GetComponent<GlobalData>();
            int playerNum = 0;
            int humanNum = 0;
            for(int i = 0; i < _playerState.Length; i++)
            {
                if (_playerState[i] < 4)
                {
                    playerNum++;
                    humanNum++;
                }
                else if (_playerState[i] != 7)
                {
                    playerNum++;
                }
            }
            globalData.SetData(playerNum, humanNum);
        }
    }

    void SetButton(int num)
    {
        _buttonList[num].Select();
        _cursor.rectTransform.sizeDelta = _buttonList[num].image.rectTransform.sizeDelta + new Vector2(15, 15);
        _cursor.rectTransform.position = _buttonList[num].image.rectTransform.position;
    }

    public void ChangeState(int n)
    {
        if (_playerState[n] == n) _playerState[n] += 4 - n;
        else _playerState[n]++;
        if (_playerState[n] >= _stateImage.Length) _playerState[n] = n;
        _buttonList[n].image.sprite = _stateImage[_playerState[n]];
    }
}

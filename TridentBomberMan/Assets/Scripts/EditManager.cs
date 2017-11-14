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
    // Use this for initialization
    void Awake () {
        _selectNum = 0;
        SetButton(_selectNum);
        _playerState = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _playerState[i] = i;
        }

        AudioController.PlayMusic("EditBgm");
    }
	
	// Update is called once per frame
	void Update () {
        if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.One))
        {
            AudioController.Play("Select");

            if (++_selectNum >= _buttonList.Length) _selectNum = 0;
            SetButton(_selectNum);
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
        }
    }

    void SetButton(int num)
    {
        _buttonList[num].Select();
        _cursor.rectTransform.sizeDelta = _buttonList[num].image.rectTransform.sizeDelta + new Vector2(10, 10);
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

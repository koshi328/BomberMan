using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GamepadInput;

public class TitleSceneManager : MonoBehaviour
{

    // アニメーションで上下させる縦幅
    private readonly float INTERVAL_Y = 30.0f;

    [SerializeField]
    private Image _imageLogo;

    [SerializeField]
    private Image _imagePress;

    [SerializeField]
    private Image[] _imageBackground;

    // シーン遷移中か？
    private bool _isChanging = false;

    private static readonly float SCROLL_SPD = 100.0f;



    void Awake()
    {
        _imageLogo.rectTransform.DOLocalPath(
            new Vector3[] { new Vector3(_imageLogo.rectTransform.localPosition.x, _imageLogo.rectTransform.localPosition.y + INTERVAL_Y),
                new Vector3(_imageLogo.rectTransform.localPosition.x, _imageLogo.rectTransform.localPosition.y) }, 10.0f, PathType.CatmullRom).SetLoops(-1);

        _imagePress.rectTransform.DOLocalPath(
            new Vector3[] { new Vector3(_imagePress.rectTransform.localPosition.x, _imagePress.rectTransform.localPosition.y + INTERVAL_Y),
                new Vector3(_imagePress.rectTransform.localPosition.x, _imagePress.rectTransform.localPosition.y) }, 5.0f, PathType.CatmullRom).SetLoops(-1);

        var go = GameObject.Find("FadeManager");
        FadeManager fadeManager = go.GetComponent<FadeManager>();
        fadeManager.FadeIn();

        AudioController.PlayMusic("TitleBgm");
    }

    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            _imageBackground[i].rectTransform.localPosition -= new Vector3(SCROLL_SPD * Time.deltaTime, 0.0f);

            if(_imageBackground[i].rectTransform.localPosition.x < -1800.0f)
            {
                _imageBackground[i].rectTransform.localPosition = new Vector3(1800.0f, 0.0f);
            }
        }

        if (_isChanging) return;

        if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.X, GamepadInput.GamePad.Index.One) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            _isChanging = true;
            AudioController.Play("Decide");

            var go = GameObject.Find("FadeManager");
            FadeManager fadeManager = go.GetComponent<FadeManager>();
            fadeManager.ChangeScene(1);
        }
    }
}

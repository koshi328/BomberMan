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

    // 待機時間
    private readonly float WAIT_TIME = 0.2f;

    [SerializeField]
    private Image _imageLogo;

    [SerializeField]
    private Image _imagePress;

    [SerializeField]
    private SpriteRenderer _fadeSprite;

    // シーン遷移中か？
    private bool _isChanging = false;

    // 時間
    private float _elapsedTime = 0.0f;



    void Awake()
    {
        _imageLogo.rectTransform.DOLocalPath(
            new Vector3[] { new Vector3(_imageLogo.rectTransform.localPosition.x, _imageLogo.rectTransform.localPosition.y + INTERVAL_Y),
                new Vector3(_imageLogo.rectTransform.localPosition.x, _imageLogo.rectTransform.localPosition.y) }, 10.0f, PathType.CatmullRom).SetLoops(-1);

        _imagePress.rectTransform.DOLocalPath(
    new Vector3[] { new Vector3(_imagePress.rectTransform.localPosition.x, _imagePress.rectTransform.localPosition.y + INTERVAL_Y),
                new Vector3(_imagePress.rectTransform.localPosition.x, _imagePress.rectTransform.localPosition.y) }, 5.0f, PathType.CatmullRom).SetLoops(-1);
    }

    void Update()
    {
        if (_isChanging) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
            _isChanging = true;
        }
    }
}

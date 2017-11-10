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

    // シーン遷移中か？
    private bool _isChanging = false;



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
            SceneManager.LoadScene("Edit", LoadSceneMode.Single);
            _isChanging = true;
        }
    }
}

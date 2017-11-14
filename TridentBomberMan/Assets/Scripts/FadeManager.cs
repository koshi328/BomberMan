using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {

    public enum STATE
    {
        NONE,
        FADE_IN,
        FADE_OUT,
    }

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private float _currentRate;
    public STATE _state;
    private int _nextScene;

	void Awake ()
    {
        _currentRate = 1.0f;
        _state = STATE.FADE_IN;
        _nextScene = 0;
        DontDestroyOnLoad(this);
	}
	
	void Update ()
    {
        switch(_state)
        {
            case STATE.NONE:
                break;
            case STATE.FADE_IN:
                _currentRate -= Time.deltaTime;
                if(_currentRate < 0.0f)
                {
                    _state = STATE.NONE;
                }
                break;
            case STATE.FADE_OUT:
                _currentRate += Time.deltaTime;
                if (1.0f <= _currentRate)
                {
                    _state = STATE.NONE;

                    // シーン遷移
                    SceneManager.LoadScene(_nextScene);

                    FadeIn();
                }
                break;
        }

        _currentRate = Mathf.Clamp(_currentRate, 0.0f, 1.0f);
        _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, _currentRate);
    }

    public void FadeIn()
    {
        _currentRate = 1.0f;
        _state = STATE.FADE_IN;
    }

    public void FadeOut()
    {
        _currentRate = 0.0f;
        _state = STATE.FADE_OUT;
    }

    public void ChangeScene(int scene)
    {
        _nextScene = scene;
        FadeOut();
    }
}

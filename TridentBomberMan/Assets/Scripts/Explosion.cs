using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour 
{
    [SerializeField]
    Animator _animator;
    

    public void Play()
    {
        if (gameObject.GetActive()) return;

        gameObject.SetActive(true);
        _animator.Play("Explosion", 0, 0.0f);
    }

    public void Vanish()
    {
        gameObject.SetActive(false);
    }
}

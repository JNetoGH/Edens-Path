using System;
using UnityEngine;

public class TalkWithDevilCutsceneSeqController : MonoBehaviour
{

    [SerializeField] private Animator _eyesEffect;
    [SerializeField] private bool _overrideCutSceneStart = false;
    
    // Animator
    private Animator _animator;
    private static readonly int StartSequence = Animator.StringToHash("StartSequence");

    // Audio
    private AudioSource _audioSource;
    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_overrideCutSceneStart)
        {
            _animator.SetTrigger(StartSequence);
            _overrideCutSceneStart = false;
        }
    }

    #region Called By The Animation Events

    public void PlayDialog()
    {
        _audioSource.Play();
    }

    public void PlayEyesOpeningEffect()
    {
        _eyesEffect.SetTrigger("SlowOpenEyes");
    }

    #endregion

    
}

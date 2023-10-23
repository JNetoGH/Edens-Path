using System;
using UnityEngine;

public class TalkWithDevilCutsceneSeqController : MonoBehaviour
{

    [SerializeField] private Animator _eyesEffect;
    [SerializeField] private bool _overrideCutSceneStart = false;
    
    // Devil
    private GameObject _devil;
    private Animator _devilAnimator;
    
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

    private void Start()
    {
        _devil = SuperTag.GetFirstObjectWithSuperTag("Devil");
        _devilAnimator = _devil.GetComponent<Animator>();
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

    private void ActivateDevilTalkingAnimation()
    {
        _devilAnimator.SetBool("Talk", true);
    }

    private void DeactivateDevilTalkingAnimation()
    {
        _devilAnimator.SetBool("Talk", false);
    }
    
    #endregion

    
}

using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This script controls the burning tree cutscene sequence.
/// It handles the cutscene and its to animation events.
/// </summary>
public class TalkWithDevilACutsceneController : ACutsceneController
{
    
    [Header("REQUIRED REFERENCES"), HorizontalLine]
    [SerializeField, Required, BoxGroup] private GameObject _devil;
    [SerializeField, Required, BoxGroup] private Animator _eyesEffect;
    [SerializeField, Required, BoxGroup] private AudioClip _lastLine;
    [SerializeField, Required, BoxGroup] private GameObject _vanishEffect;
    [SerializeField, Required, BoxGroup] private Transform _playerPositionOnCutscene;
    
    // Devil
    private Animator _devilAnimator;
    
    // Player
    private GameObject _player;
    
    // Audio
    private AudioSource _audioSource;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _devilAnimator = _devil.GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>().gameObject;
        
        // In case the cutscene has been already watched,simply won't play the cutscene.
        if (!hasBeenAlreadyWatched)
        {
            PlayCutscene();
        }
        if (hasBeenAlreadyWatched)
        {
            DisableDevil();
            GameManager.SkipCutsceneMsg.SetActive(false);
        }
    }

    private void Update()
    {
        bool isRunning = animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        if (isRunning && Input.GetKeyDown(KeyCode.Return))
        {
            animator.enabled = false;
            DisableDevil();
            _audioSource.Stop();
            _eyesEffect.gameObject.SetActive(false);
            GameManager.EnterGameplayMode();
            GameManager.SkipCutsceneMsg.SetActive(false);
        }
    }

    public override void SkipCutscene()
    {
        DisableDevil();
        GameManager.EnterGameplayMode();
    }
    
    public override void PlayCutscene()
    {
        _player.transform.position = _playerPositionOnCutscene.position;
        animator.SetTrigger("StartSequence");
        _devil.SetActive(true);
        _vanishEffect.SetActive(false);
    }
    
    #region Called By Animation Events

        private void PlayDialog()
        {
            _audioSource.Play();
        }

        private void PlayEyesOpeningEffect()
        {
            _eyesEffect.SetTrigger("SlowOpenEyes");
        }

        private void PlayLastLine()
        {
            _audioSource.clip = _lastLine;
            _audioSource.Play();
        }

        private void VanishEffectOnDevil()
        {
            _vanishEffect.SetActive(true);
        }
        
        // last event called, changes the hasBeenAlreadyWatched to true.
        private void DisableDevil()
        {
            _devil.SetActive(false);
            _vanishEffect.SetActive(false);
            hasBeenAlreadyWatched = true;
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

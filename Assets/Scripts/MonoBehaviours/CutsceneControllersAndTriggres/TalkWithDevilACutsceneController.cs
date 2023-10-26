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
    [BoxGroup, Required, SerializeField] private GameObject _devil;
    [BoxGroup, Required, SerializeField] private Animator _eyesEffect;
    [BoxGroup, Required, SerializeField] private AudioClip _lastLine;
    [BoxGroup, Required, SerializeField] private GameObject _vanishEffect;
    [BoxGroup, Required, SerializeField] private Transform _playerPositionOnCutscene;
    
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
        
        if (!hasAlreadySeenThisCutscene)
            PlayCutscene();
        
        if (hasAlreadySeenThisCutscene)
            DisableDevil();
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
        
        private void DisableDevil()
        {
            _devil.SetActive(false);
            _vanishEffect.SetActive(false);
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

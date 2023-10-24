using UnityEngine;
using UnityEngine.Serialization;


public class TalkWithDevilCutsceneController : MonoBehaviour
{
    
    [Header("Overriding")]
    [SerializeField] private bool _overrideTalkWithDevilCutscenePlay = false;

    [Header("Controlling")] 
    [SerializeField] private bool _hasAlreadySeenTheFirstCutscene = false;
    
    [Header("References")]
    [SerializeField] private GameObject _devil;
    [SerializeField] private Animator _eyesEffect;
    [SerializeField] private AudioClip _lastLine;
    [SerializeField] private GameObject _vanishEffect;
    [SerializeField] private Transform _playerPositionOnCutscene;
    
    // Devil
    private Animator _devilAnimator;
    
    // Player
    private GameObject _player;
    
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
        _devilAnimator = _devil.GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>().gameObject;
        
        if (!_hasAlreadySeenTheFirstCutscene)
            StartCutscene();
        
        if (_hasAlreadySeenTheFirstCutscene)
            DisableDevil();
    }

    private void Update()
    {
        if (_overrideTalkWithDevilCutscenePlay)
        {
            StartCutscene();
            _overrideTalkWithDevilCutscenePlay = false;
        }
    }

    public void StartCutscene()
    {
        _player.transform.position = _playerPositionOnCutscene.position;
        _animator.SetTrigger(StartSequence);
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
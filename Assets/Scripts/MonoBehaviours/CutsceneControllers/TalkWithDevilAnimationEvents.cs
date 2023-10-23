using UnityEngine;
using UnityEngine.Serialization;


public class TalkWithDevilAnimationEvents : MonoBehaviour
{
    
    [Header("Overriding")]
    [SerializeField] private bool _overrideTalkWithDevilCutscenePlay = false;

    [Header("Controlling")] 
    [SerializeField] private bool _hasAlreadySeenTheFirstCutscene = false;
    
    [Header("References")]
    [SerializeField] private GameObject _devil;
    private Animator _devilAnimator;
    
    [Header("Settings")]
    [SerializeField] private Animator _eyesEffect;
    [SerializeField] private AudioClip _lastLine;
    [SerializeField] private GameObject _vanishEffect;
    [SerializeField] private Transform _playerPositionOnCutscene;
    
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
    
    #region Called By The Animation Events

    public void PlayDialog()
    {
        _audioSource.Play();
    }

    public void PlayEyesOpeningEffect()
    {
        _eyesEffect.SetTrigger("SlowOpenEyes");
    }

    public void PlayLastLine()
    {
        _audioSource.clip = _lastLine;
        _audioSource.Play();
    }

    public void VanishEffectOnDevil()
    {
        _vanishEffect.SetActive(true);
    }
    
    public void DisableDevil()
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

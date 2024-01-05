using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class BatteryPuzzleController : ACutsceneController
{

    [Header("Battery")]
    [SerializeField, Tag] private string _batteryTag;
    [SerializeField] private Transform _batteryPosition;

    [Header("Cutscene")] 
    [SerializeField] private float _playCutsceneAfter = 1.5f;
    [SerializeField] private float _cutsceneDuration = 7f;
    [SerializeField] private float _playerVoiceAt = 1.2f;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    
    private List<SpotlightController> _spotlightControllers;
    private CinemachineVirtualCamera _virtualCamera;
    private AudioSource _audioSource;

    private bool CinemachineSetUp
    {
        set
        {
            _virtualCamera.enabled = value;
            _cinemachineBrain.gameObject.SetActive(value);
        }
    }
    
    private void Start()
    {
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _spotlightControllers = FindObjectsOfType<SpotlightController>().ToList();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!HasBeenAlreadyWatched) 
            return;
        
        hasBeenAlreadyWatched = true;
        LockBatteryInPlace();
        TurnSpotlightsOn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals(_batteryTag))
            return;

        if (HasBeenAlreadyWatched)
            return;
        
        LockBatteryInPlace();
        Invoke(nameof(PlayCutscene), _playCutsceneAfter);
        hasBeenAlreadyWatched = true;
    }
    
    public override void PlayCutscene()
    {
        TurnSpotlightsOn();
        GameManager.EnterCutsceneMode();
        CinemachineSetUp = true;
        Invoke(nameof(PlayPlayerLine), _playerVoiceAt);
        Invoke(nameof(EndCutsceneCoroutine), _cutsceneDuration);
    }

    private void LockBatteryInPlace()
    {
        GameObject b = GameObject.FindWithTag(_batteryTag);
        b.GetComponent<PickableObject>().enabled = false;
        b.GetComponent<Outline>().enabled = false;
        b.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        b.transform.rotation = _batteryPosition.rotation;
        b.transform.position = _batteryPosition.position;
    }
    
    private void TurnSpotlightsOn()
    {
        _spotlightControllers.ForEach(c => c.enabled = true);
    }
    
    private void PlayPlayerLine()
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }
    
    private void EndCutsceneCoroutine()
    {
        CinemachineSetUp = false;
        GameManager.EnterGameplayMode();
    }
    
}

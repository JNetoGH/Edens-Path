using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


public class FloraInteraction2CutsceneController : ACutsceneController
{
    
    private float _timer = 0f;
    private const float TimerDuration = 4;
    private bool _updateTimer = false;
    
    private const string G1 = "DIALOG 2";
    //[SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteraction2;
    //[SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteractionMsg;
    [SerializeField, Required, BoxGroup(G1)] private GameObject _seed;
    [SerializeField, Required, BoxGroup(G1)] private Transform _seedPositionWithFlora;
    [SerializeField, Required, BoxGroup(G1)] private AudioClip _floraDialog2;
    [SerializeField, Required, BoxGroup(G1)] private GameObject _bouquetPrefab;
    [SerializeField, Required, BoxGroup(G1)] private Transform _bouquetInstantiationPosition;
  
    
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_updateTimer)
            _timer += Time.deltaTime;
        
        if (_timer >= TimerDuration)
        {
            // Instantiates the flowers
            GameObject bouquet = Instantiate(_bouquetPrefab);
            bouquet.transform.position = _bouquetInstantiationPosition.position;
            
            _timer = 0;
            _updateTimer = false;
        }
    }
    
    public override void PlayCutscene()
    {
        // Disables interaction 1
        GetComponentInChildren<FloraInteraction1CutsceneController>().DisableFloraNpcInteractionTrigger1();
        
        // Locks seed with flora.
        _seed.transform.parent = _seedPositionWithFlora;
        _seed.transform.localScale = new Vector3(5f, 5f, 5f);
        _seed.transform.localRotation = Quaternion.identity;
        _seed.transform.localPosition = Vector3.zero;
        _seed.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        _seed.GetComponent<PickableObject>().enabled = false;
        _seed.GetComponent<Outline>().enabled = false;
        _seed.GetComponent<Collider>().enabled = false;

        // sets the timer to give the flowers
        _updateTimer = true;
        
        PlayDialog2Audio();
        DisableFloraNpcInteractionTrigger1();
    }
    
    #region Animation Events
    
        public void PlayDialog2Audio()
        {
            _audioSource.clip = _floraDialog2;
            _audioSource.Play();
        }
        
        public void DisableFloraNpcInteractionTrigger1()
        {
            //_floraNpcInteraction2.SetActive(false);
            //_floraNpcInteractionMsg.SetActive(false);
        }
        
    #endregion
    
}

using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


public class FloraInteraction2CutsceneController : ACutsceneController
{
    
    
    private const string G1 = "DIALOG 2";
    //[SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteraction2;
    //[SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteractionMsg;
    [SerializeField, Required, BoxGroup(G1)] private GameObject _seed;
    [SerializeField, Required, BoxGroup(G1)] private Transform _seedPositionWithFlora;
    [SerializeField, Required, BoxGroup(G1)] private AudioClip _floraDialog2;
    
    
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            GetComponentInChildren<FloraInteraction1CutsceneController>().DisableFloraNpcInteractionTrigger1();
    }
    
    public override void PlayCutscene()
    {
        // animator.SetTrigger("NextCutscutscene");
        
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

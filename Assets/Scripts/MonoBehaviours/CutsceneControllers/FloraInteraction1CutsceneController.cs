using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;


public class FloraInteraction1CutsceneController : ACutsceneController
{
    
    private const string G1 = "DIALOGUE 1";
    [SerializeField, Required, BoxGroup(G1)] private AudioClip _floraDialogue1;
    [SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteraction1;
    [SerializeField, Required, BoxGroup(G1)] private GameObject _floraNpcInteractionMsg;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    
    public override void PlayCutscene()
    {
        animator.SetTrigger("NextCutscutscene");
        PlayDialog1Audio();
        DisableFloraNpcInteractionTrigger1();
    }
    
    #region Animation Events
    
        public void PlayDialog1Audio()
        {
            _audioSource.clip = _floraDialogue1;
            _audioSource.Play();
        }
        
        public void DisableFloraNpcInteractionTrigger1()
        {
            _floraNpcInteraction1.SetActive(false);
            _floraNpcInteractionMsg.SetActive(false);
        }
        
    #endregion
    
}

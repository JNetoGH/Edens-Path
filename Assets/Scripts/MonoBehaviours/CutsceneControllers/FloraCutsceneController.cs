using NaughtyAttributes;
using UnityEngine;


public class FloraCutsceneController : ACutsceneController
{
    
    [SerializeField, Required, BoxGroup] private GameObject _flora;
    [SerializeField, Required, BoxGroup] private AudioClip _floraDialogue1;
    [SerializeField, Required, BoxGroup] private GameObject _floraNpcInteraction1;
    [SerializeField, Required, BoxGroup] private GameObject _floraNpcInteractionMsg;
    
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

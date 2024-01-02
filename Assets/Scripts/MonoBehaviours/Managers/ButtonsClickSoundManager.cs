using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class ButtonsClickSoundManager : MonoBehaviour
{

    public bool Mute { get; set; }

    [SerializeField] private AudioClip _clickSound;
    private AudioSource _audioSource;
    private List<Button> _buttons;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _buttons = FindObjectsOfType<Button>(true).ToList();
        _buttons.ForEach(b => b.onClick.AddListener(PlayButtonClickSound));
    }
    
    private void PlayButtonClickSound() {
        if (!Mute)
            _audioSource.PlayOneShot(_clickSound);
    }
    
}

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundTrackController : MonoBehaviour
{
    
    [SerializeField] private bool _playTracks;
    [SerializeField] private bool _loopTrack;
    [SerializeField] private int _playingTrack;
    [SerializeField] private AudioClip[] _audioTracks;
    private AudioSource _audioSource;
    
    public bool IsMute { get; private set; }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        IsMute = PlayerPrefs.GetInt("_Mute", 0) == 1;
        _audioSource.mute = IsMute;
        
        if (_audioTracks.Length <= 0 || IsMute) 
            return;
        
        _audioSource.clip = _audioTracks[0];
        _audioSource.Play();
    }

    void Update()
    {
        HandleTrackPlayback();
    }

    /// <summary>
    /// Toggle the mute state
    /// </summary>
    public void ToggleMute()
    {
        IsMute = !IsMute; 
        _audioSource.mute = IsMute;
        PlayerPrefs.SetInt("_Mute", IsMute ? 1 : 0);
    }
    
    private void HandleTrackPlayback()
    {
        _audioSource.loop = _loopTrack;
        if (!_playTracks && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        else if (_playTracks && !_audioSource.isPlaying)
        {
            StartPlayer();
        }
    }

    private void StartPlayer()
    {
        if (_audioTracks.Length > 0)
        {
            _audioSource.clip = _audioTracks[_playingTrack];
            _audioSource.Play();
        }
    }
}

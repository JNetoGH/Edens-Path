using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Validates the progression for the first level.
/// </summary>
public class LevelOneProgressValidator : MonoBehaviour, ILevelProgressValidator
{

    [SerializeField] private GameObject _progressionCollider;
    [HideInInspector] public bool hasDrinhaGotTheFlowers = false;
    private ACutsceneController _bridgeCutscene;
    
    private void Start()
    {
        // Subscribes this validator to the handler
        hasDrinhaGotTheFlowers = false;
        CallAtStartAndSubscribeToHandler(GetComponent<LevelProgressHandler>());
        _bridgeCutscene = FindObjectOfType<FirstLevelBridgeAnimationController>();
    }
    
    // Comes From the Interface.
    public void OnValidation(LevelProgressHandler handler)
    {
        if (hasDrinhaGotTheFlowers || _bridgeCutscene.HasBeenAlreadyWatched)
            handler.HasProgressed = true;
        
        // disables/enables the collider that is preventing the player from scape the first island.
        _progressionCollider.SetActive(!handler.HasProgressed);
    }
    
    // Comes From the Interface.
    public void OnProgression()
    {   
        // In case the cutscene has been already watched, it will give only a msg.
        if (_bridgeCutscene.HasBeenAlreadyWatched)
        {
            Debug.Log("Level 1 progression cutscene won't be played because it has been already watched");
        }
        else
        {
            _bridgeCutscene.PlayCutscene();
            Debug.Log("Level Succeed");
        }
    }

    // Comes From the Interface.
    public void CallAtStartAndSubscribeToHandler(LevelProgressHandler handler)
    {
        if (handler == null)
            Debug.LogError($"Tried to subscribe a ILevelProgressValidator to a null LevelProgressionHandler");
        handler.iLevelProgressValidator = this;
    }
    
}
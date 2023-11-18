using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Validates the progression for the first level.
/// </summary>
public class LevelOneProgressValidator : MonoBehaviour, ILevelProgressValidator
{

    [HideInInspector] public bool hasDrinhaGotTheFlowers = false; 
    
    private void Start()
    {
        // Subscribes this validator to the handler
        hasDrinhaGotTheFlowers = false;
        CallAtStartAndSubscribeToHandler(GetComponent<LevelProgressHandler>());
    }
    
    // Comes From the Interface.
    public void OnValidation(LevelProgressHandler handler)
    {
        if (hasDrinhaGotTheFlowers)
            handler.HasProgressed = true;
    }
    
    // Comes From the Interface.
    public void OnProgression()
    {   
        ACutsceneController cutscene = FindObjectOfType<FirstLevelBridgeAnimationController>();
        
        // In case the cutscene has been already watched, it will give only a msg.
        if (cutscene.HasBeenAlreadyWatched)
        {
            Debug.Log("Level 1 progression cutscene won't be played because it has been already watched");
        }
        else
        {
            cutscene.PlayCutscene();
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
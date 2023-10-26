using UnityEngine;

/// <summary>
/// Validates the progression for the first level.
/// </summary>
public class LevelOneProgressValidator : MonoBehaviour, ILevelProgressValidator
{
    
    private void Start()
    {
        // Subscribes this validator to the handler
        CallAtStartAndSubscribeToHandler(GetComponent<LevelProgressHandler>());
    }
    
    // Comes From the Interface.
    public void OnValidation(LevelProgressHandler handler)
    {
        // when validates the progression.
        // handler.HasProgressed = true;
    }
    
    // Comes From the Interface.
    public void OnProgression()
    {   
        FindObjectOfType<FirstLevelBridgeAnimationController>().PlayCutscene();
        Debug.Log("Level Succeed");
    }

    // Comes From the Interface.
    public void CallAtStartAndSubscribeToHandler(LevelProgressHandler handler)
    {
        if (handler == null)
            Debug.LogError($"Tried to subscribe a ILevelProgressValidator to a null LevelProgressionHandler");
        handler.iLevelProgressValidator = this;
    }
    
}
using UnityEngine;

public class LevelProgressHandler : MonoBehaviour
{
    
    /// <summary>
    /// Indicates whether the level has progressed.
    /// This property is manipulated by the validation event listeners.
    /// </summary>
    public bool HasProgressed { get; set; } = false;
    [SerializeField] private bool _overrideLevelCompletion = false;
    
    /// <summary>
    /// A script used to implement the progress validation of this level
    /// </summary>
    public ILevelProgressValidator iLevelProgressValidator;

    private void Start()
    {
        // Initialize HasProgressed to false when the level starts.
        HasProgressed = false;
        
        // Check if the is a ILevelProgress assigned
        if (iLevelProgressValidator == null) 
            Debug.LogWarning($"LevelProgressionHandler at {gameObject.name} has no ILevelProgressValidator assigned");
    }

    private void Update()
    {
        // If overridden level completion, sets HasProgressed to true and trigger the progress event.
        if (_overrideLevelCompletion)
        {
            HasProgressed = true;
            _overrideLevelCompletion = false;
            iLevelProgressValidator.OnProgression();
        }

        // If the level has already progressed, there's no need to check validation or invoke progress.
        if (HasProgressed)
            return;

        // Validate the progression using the listeners by passing itself as an argument,
        // so they can change the HasProgressed property.
        iLevelProgressValidator.OnValidation(this);

        // If the validation has occurred, call the listeners of this event to signify progress.
        if (HasProgressed)
            iLevelProgressValidator.OnProgression();
    }
}
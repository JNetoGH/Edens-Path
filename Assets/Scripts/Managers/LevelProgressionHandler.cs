using UnityEngine;
using UnityEngine.Events;

public class LevelProgressionHandler : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the level has progressed.
    /// This property is manipulated by the validation event listeners.
    /// </summary>
    public bool HasProgressed { get; set; } = false;

    [SerializeField] private bool _overrideLevelCompletion = false;
    [SerializeField] private UnityEvent<LevelProgressionHandler> _onLevelValidation;
    [SerializeField] private UnityEvent _onLevelProgress;

    private void Start()
    {
        // Initialize HasProgressed to false when the level starts.
        HasProgressed = false;
    }

    private void Update()
    {
        // If overridden level completion, sets HasProgressed to true and trigger the progress event.
        if (_overrideLevelCompletion)
        {
            HasProgressed = true;
            _overrideLevelCompletion = false;
            _onLevelProgress.Invoke();
        }

        // If the level has already progressed, there's no need to check validation or invoke progress.
        if (HasProgressed)
            return;

        // Validate the progression using the listeners by passing itself as an argument,
        // so they can change the HasProgressed property.
        _onLevelValidation.Invoke(this);

        // If the validation has occurred, call the listeners of this event to signify progress.
        if (HasProgressed)
            _onLevelProgress.Invoke();
    }
}
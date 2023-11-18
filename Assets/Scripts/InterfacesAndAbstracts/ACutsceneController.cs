using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// An abstract class representing a generic cutscene controller.
/// Provides functionality for playing a cutscene through an Animator component.
/// </summary>
public abstract class ACutsceneController : MonoBehaviour
{
    /// <summary>
    /// Indicates whether this cutscene has already been watched or not.
    /// It's used mainly to force the cutscenes to load at their completion state,
    /// and not be able to be triggered anymore.
    /// </summary>
    [HorizontalLine]
    [BoxGroup("CONTROLLING"), SerializeField] public bool hasBeenAlreadyWatched = false;
    public bool HasBeenAlreadyWatched => hasBeenAlreadyWatched;

    /// <summary>
    /// A button that appears automatically in the editor to force the cutscene to play.
    /// </summary>
    [Button]
    protected void OverrideCutscenePlay() => PlayCutscene();

    /// <summary>
    /// The Animator responsible for controlling the cutscene.
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// Plays the cutscene. This method can be triggered by other GameObjects.
    /// </summary>
    public abstract void PlayCutscene();
}
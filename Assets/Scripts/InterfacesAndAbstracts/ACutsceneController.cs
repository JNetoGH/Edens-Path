using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// An abstract class representing a generic cutscene controller.
/// Provides functionality for playing a cutscene through an Animator component.
/// </summary>
public abstract class ACutsceneController : MonoBehaviour
{
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
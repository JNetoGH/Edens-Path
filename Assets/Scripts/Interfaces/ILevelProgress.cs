public interface ILevelProgress
{
    /// <summary>
    /// Called by the the LevelProgressionHandler during the level progression validation process.
    /// </summary>
    /// <param name="handler">
    /// The LevelProgressionHandler has a HasProgressed public property that is used to control the OnProgression call.
    /// </param>
    public abstract void OnValidation(LevelProgressionHandler handler);
    
    /// <summary>
    /// Called by the the LevelProgressionHandler during the level progression process,
    /// called when the HasProgressed property is set true.
    /// </summary>
    public abstract void OnProgression();
}

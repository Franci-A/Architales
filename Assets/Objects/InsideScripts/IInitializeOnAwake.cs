/// <summary>
/// <see cref="Initialize"/> method Called on Awake
/// by the <see cref="Initializer"/>, for every Object referenced in its list
/// </summary>
public interface InitializeOnAwake
{
    /// <summary>
    /// Set all the starting parameters, events needed.
    /// Method Called on Awake
    /// </summary>
    public void Initialize();
}

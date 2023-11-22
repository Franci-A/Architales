/// <summary>
/// <see cref="Uninitialize"/> method Called on Awake
/// by the <see cref="Initializer"/>, for every Object referenced in its list
/// </summary>
public interface UninitializeOnDisable
{
    /// <summary>
    /// Reset all the parameters, events to be destroyed.
    /// Method Called on Disable
    /// </summary>
    public void Uninitialize();
}

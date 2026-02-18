/// <summary>
/// Shared static state name — action tasks set this in OnExecute,
/// and ParrotHUD reads it every frame to display the current state.
/// </summary>
public static class ParrotState {
    public static string Current = "Idle";
}

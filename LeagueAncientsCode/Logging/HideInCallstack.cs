namespace LeagueAncients.Logging;

/// <summary>
/// Hides the method or class in optional stack traces when printing a Warning
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HideInCallstackAttribute : Attribute
{
    public HideInCallstackAttribute() { }
}
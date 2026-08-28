

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LeagueAncients.Logging;

internal static class ModLog
{
    private static readonly Logger Logger = new (MainFile.MOD_ID, LogTopic.All);

    public static string Timestamp => DateTime.UtcNow.ToString("HH:mm:ss");

    public static event Action<LogLevel, string, int>? LogCallback;

    public static void InvokeGlobalLogCallback(LogLevel logLevel, string log, int skipFrames) => LogCallback?.Invoke(logLevel, log, skipFrames);

    
    /// <summary> Prints to stdout. It should be used when loading operations are happening. </summary>
    [StackTraceHidden]
    public static void Load(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.Load(text, topic, skipFrames, callerFilePath, callerMemberName);

    /// <summary> Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text. </summary>
    [StackTraceHidden]
    public static void Debug(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.Debug(text, topic, skipFrames, callerFilePath, callerMemberName);

    /// <summary> Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text. </summary>
    [StackTraceHidden]
    public static void VeryDebug(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.VeryDebug(text, topic, skipFrames, callerFilePath, callerMemberName);

    /// <summary> Prints to stdout. It should be used for general information which is useful for debugging. </summary>
    [StackTraceHidden]
    public static void Info(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.Info(text, topic, skipFrames, callerFilePath, callerMemberName);

    /// <summary> Prints to stderr without a stacktrace. It should be used for non-critical issues which we should be aware of
    /// or could indicate an issue. </summary>
    [StackTraceHidden]
    public static void Warn(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.Warn(text, topic, skipFrames, callerFilePath, callerMemberName);

    /// <summary> Prints a stacktrace to stderr. It should be used for critical issue which should also not block
    /// the continuation of the game. </summary>
    [StackTraceHidden]
    public static void Error(string text, LogTopic topic = LogTopic.Default,  int skipFrames = 2, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Logger.Error(text, topic, skipFrames, callerFilePath, callerMemberName);

}
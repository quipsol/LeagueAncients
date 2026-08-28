using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LeagueAncients.Logging;

public interface IPrintLog
{
    string MessageIsNullInfoText { get; }
    string MessageIsEmptyInfoText { get; }
    [StackTraceHidden]
    void Print(LogLevel logLevel, LogTopic logTopic, string? context, string text, int skipFrames, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0);
    
    [StackTraceHidden]
    void Print(LogLevel logLevel, LogTopic logTopic, string? context, object obj, int skipFrames, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        => Print(logLevel, logTopic, context, obj.ToString() ?? MessageIsNullInfoText, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
}
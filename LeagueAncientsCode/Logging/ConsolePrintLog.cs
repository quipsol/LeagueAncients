using System.Diagnostics;
using System.Runtime.CompilerServices;
using Godot;

namespace LeagueAncients.Logging;

public class ConsolePrintLog : IPrintLog
{
    public string MessageIsNullInfoText => "Null";
    public string MessageIsEmptyInfoText => "String.Empty";
    
    [StackTraceHidden]
    public void Print(LogLevel logLevel, LogTopic logTopic, string? context, string text, int skipFrames, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
    {
        skipFrames++;
        var logLevelText = $"{logLevel.ToString().ToUpperInvariant()}";
        var callerName = $"[Caller: {Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMemberName}]";
        var fullContextText = (context != null ? $"[{context}] " : "") + $"[{logTopic.ToString()}] {callerName}";
        
        var message = (fullContextText + text).Length > 200 
                    ? $"[{logLevelText}] {fullContextText}\n\t{text}" 
                    : $"[{logLevelText}] {fullContextText} {text}";
        
        switch (logLevel)
        {
            case LogLevel.Error:
                {
                    var trace = new StackTrace(fNeedFileInfo: true);
                    GD.PrintErr($"{message}\n{trace}");
                }
                break;
            case LogLevel.Warn:
                {
                    var trace = new StackTrace(fNeedFileInfo: true);
                    var filteredTrace = LogHelper.FilterHideInCallstack(trace, 3);
                    GD.Print($"{message}\n{filteredTrace}");
                }
                break;
            default:
                GD.Print($"{message}");
                break;
        }
    }
}
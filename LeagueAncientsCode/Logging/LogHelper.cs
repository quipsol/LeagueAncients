using System.Diagnostics;
using System.Reflection;
using System.Text;
using Godot;

namespace LeagueAncients.Logging;

public static class LogHelper
{
    // Notes:
    // 1.
    // This intentionally relies on StackTrace.ToString() producing one output
    // line per StackFrame. If the runtime formatting changes, this mapping may
    // need to be revisited.
    // 2.
    // If this is too resource intensive, outsource it to another thread.
    
    /// <summary>
    /// Filters stack frames with <see cref="HideInCallstackAttribute"/>
    /// </summary>
    /// <param name="trace">The StackTrace</param>
    /// <param name="maxFrames">Optional maximum frame count</param>
    /// <returns>The formatted string (based on StackTrace.ToString)</returns>
    [StackTraceHidden]
    public static string FilterHideInCallstack(StackTrace trace, int maxFrames = -1)
    {
        var frames = trace.GetFrames().ToList();
        if (frames.Count == 0) return string.Empty;
        for (var i = frames.Count - 1; i >= 0; i--)
        {
            var frame = frames[i];
            var method = frame.GetMethod();
            if (method is null) continue;
            var isHidden = method.IsDefined(typeof(HideInCallstackAttribute), inherit: false) ||
                            (method.DeclaringType?.IsDefined(typeof(HideInCallstackAttribute), inherit: false) ?? false);
            if (isHidden) frames.RemoveAt(i);
        }
        // StackTraceHidden is filtered inside StackTrace.ToString() so we need to limit frame count after.
        var finalTraceString = new StackTrace(frames).ToString();
        if (maxFrames >= 0)
            finalTraceString = string.Join(System.Environment.NewLine, 
                        finalTraceString
                                    .Split([System.Environment.NewLine], StringSplitOptions.None)
                                    .Take(maxFrames)
                        );
        
        return finalTraceString;
    }
}
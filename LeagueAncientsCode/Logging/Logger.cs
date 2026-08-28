using System.Diagnostics;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.TestSupport;

namespace LeagueAncients.Logging;

public class Logger
{
	private static readonly object LockObj = new object();

	private static readonly bool IsRunningFromGodotEditor = GetIsRunningFromGodotEditor();

	public static LogLevel GlobalLogLevel { get; set; } = LogLevel.Info;

	private static readonly IPrintLog LogPrinter = new ConsolePrintLog();

	private readonly LogTopic _logTopic;

	public static readonly Dictionary<LogTopic, LogLevel> LogLevelTypeMap = new Dictionary<LogTopic, LogLevel>
	{
		{ LogTopic.All, LogLevel.Info},
	};

	public string? Context { get; set; }

	public event Action<LogLevel, string, int>? LogCallback;

	private static bool GetIsRunningFromGodotEditor()
	{
		string[] cmdlineArgs = OS.GetCmdlineArgs();
		bool flag = cmdlineArgs.Any((string arg) => arg == "--headless");
		bool result = OS.HasFeature("editor");
		if (flag || TestMode.IsTestRunFromCmdline() || TestMode.IsOn)
		{
			return false;
		}
		return result;
	}

	static Logger()
	{
		string[] commandLineArgs = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i] == "-modlog")
			{
				if (!LogConsoleCmd.TryParseEnumCaseInsensitive(commandLineArgs[i + 1], out LogTopic? enumVal))
				{
					LogPrinter.Print(LogLevel.Error, LogTopic.All, null, "Invalid log command line argument! Could not parse " + commandLineArgs[i + 1] + " as LogType", 1);
				}
				if (!LogConsoleCmd.TryParseEnumCaseInsensitive(commandLineArgs[i + 2], out LogLevel? enumVal2))
				{
					LogPrinter.Print(LogLevel.Error, LogTopic.All, null, "Invalid log command line argument! Could not parse " + commandLineArgs[i + 2] + " as LogLevel", 1);
				}
				LogLevelTypeMap[enumVal.Value] = enumVal2.Value;
				LogPrinter.Print(LogLevel.Info, LogTopic.All, null, $"Log level for {enumVal} set to {enumVal2}", 1);
			}
		}
	}

	public Logger(string? context, LogTopic logTopic)
	{
		Context = context;
		_logTopic = logTopic;
		LogCallback += ModLog.InvokeGlobalLogCallback;
	}

	[StackTraceHidden]
	public bool WillLogLevel(LogLevel level) => level >= GlobalLogLevel;

	[StackTraceHidden]
	public void LogMessage(LogLevel level, LogTopic topic, string text, int skipFrames, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
	{
		if (!WillLogLevel(level) || !CheckForMatchAny(_logTopic, topic)) return;
		skipFrames++;
		LogPrint(level, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
	}
	
	[StackTraceHidden]
	private void LogPrint(LogLevel level, LogTopic topic, string text, int skipFrames, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
	{
		skipFrames++;
		lock (LockObj)
		{
			LogPrinter.Print(level, topic, Context, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
			this.LogCallback?.Invoke(level, text, skipFrames);
		}
	}
	
	#region Log Methods
	
	/// Prints to stdout. It should be used when loading operations are happening.
	[StackTraceHidden]
	public void Load(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.Load, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
	
	/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
	[StackTraceHidden]
	public void Debug(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.Debug, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
	
	/// Prints to stdout. Debug information which is useful for debugging. It should be used for verbose text.
	[StackTraceHidden]
	public void VeryDebug(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.VeryDebug, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
	
	/// Prints to stdout. It should be used for general information which is useful for debugging.
	[StackTraceHidden]
	public void Info(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.Info, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);

	/// Prints to stderr without a stacktrace. It should be used for non-critical issues which we should be aware of
	/// or could indicate an issue.
	[StackTraceHidden]
	public void Warn(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.Warn, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);

	/// Prints a stacktrace to stderr. It should be used for critical issue which should also not block
	/// the continuation of the game.
	[StackTraceHidden]
	public void Error(string text, LogTopic topic,  int skipFrames = 1, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
		=> LogMessage(LogLevel.Error, topic, text, skipFrames, callerFilePath, callerMemberName, callerLineNumber);
		
	#endregion Log Methods
	
	#region Helper
	[StackTraceHidden]
	private static bool CheckForMatchAny(LogTopic validEnum, LogTopic testEnum) => (validEnum & testEnum) != 0;
	[StackTraceHidden]
	private static bool CheckForMatchAll(LogTopic validEnum, LogTopic testEnum) => validEnum == testEnum;
	
	// Old code when enums where actually generics
	protected bool CheckForMatchAny(Enum validEnum, Enum testEnum)
	{
		if(validEnum.GetType() != testEnum.GetType())
		{
			LogPrint(LogLevel.Error, LogTopic.All, ("Type mismatch: " + validEnum.GetType() + " does not match " + testEnum.GetType()), 1);
			return false;
		}
		// TODO: Simplify
		// return validEnum.HasFlag(testEnum);
		long validSigned = Convert.ToInt64(validEnum);
		long testSigned = Convert.ToInt64(testEnum);
		ulong valid = unchecked((ulong)validSigned);
		ulong test = unchecked((ulong)testSigned);
		return (valid & test) != 0;
	}
	
	#endregion Helper
	
	public static void SetLogLevelForType(LogTopic topic, LogLevel? logLevel)
	{
		if (logLevel.HasValue)
		{
			LogLevelTypeMap[topic] = logLevel.Value;
		}
		else
		{
			LogLevelTypeMap.Remove(topic);
		}
	}
}

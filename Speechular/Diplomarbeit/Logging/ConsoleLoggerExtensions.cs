using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

using System.Runtime.CompilerServices;

namespace Diplomarbeit.Logging;

public static class ConsoleLoggerExtensions
{
  public static void Log(this ILoggerFactory loggerFactory, object obj, [CallerMemberName] string category = "App")
    => loggerFactory.CreateLogger(category).LogInformation(obj.ToString());
  public static ILoggingBuilder AddCustomFormatter(this ILoggingBuilder builder)
    => builder.AddConsole(options => options.FormatterName = nameof(CustomLoggingFormatter))
          .AddConsoleFormatter<CustomLoggingFormatter, ConsoleFormatterOptions>();
}
public sealed class CustomLoggingFormatter : ConsoleFormatter, IDisposable
{
  private readonly IDisposable? _optionsReloadToken;
  private ConsoleFormatterOptions _formatterOptions;
  public CustomLoggingFormatter(IOptionsMonitor<ConsoleFormatterOptions> options) : base(nameof(CustomLoggingFormatter))
    => (_optionsReloadToken, _formatterOptions) = (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

  private void ReloadLoggerOptions(ConsoleFormatterOptions options) => _formatterOptions = options;

  public override void Write<TState>(
    in LogEntry<TState> logEntry,
    IExternalScopeProvider? scopeProvider,
    TextWriter textWriter)
  {
    string? message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
    if (message is null) return;

    Console.BackgroundColor = ConsoleColor.Gray;
    Console.ForegroundColor = ConsoleColor.Black;
    string timestamp = DateTime.Now.ToString(_formatterOptions.TimestampFormat ?? "HH:mm:ss");
    Console.Write(timestamp);
    Console.ResetColor();
    Console.Write(" ");

    Console.ForegroundColor = ColorOf(logEntry.LogLevel);
    Console.Write(logEntry.LogLevel.ToString()[..4].ToLower());
    Console.ResetColor();
    Console.Write(" ");

    Console.Write($"{message} ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    if (logEntry.Exception != null) Console.WriteLine(logEntry.Exception);
    Console.WriteLine(logEntry.Category);
    Console.ResetColor();
  }

  private static ConsoleColor ColorOf(LogLevel level)
  {
    return level switch
    {
      LogLevel.Error => ConsoleColor.Red,
      LogLevel.Warning => ConsoleColor.Yellow,
      LogLevel.Information => ConsoleColor.DarkGreen,
      _ => ConsoleColor.White,
    };
  }

  public void Dispose() => _optionsReloadToken?.Dispose();
}

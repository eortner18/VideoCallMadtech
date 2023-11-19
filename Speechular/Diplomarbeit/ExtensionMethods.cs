using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Diplomarbeit;

public static class ExtensionMethods
{
  [Obsolete("CopyPropertiesFrom is deprecated, please use CopyFrom instead.")]
  public static T CopyPropertiesFrom<T>(this T target, object source) => CopyFrom<T>(target, source, null);
  [Obsolete("CopyPropertiesFrom is deprecated, please use CopyFrom instead.")]
  public static T CopyPropertiesFrom<T>(this T target, object source, string[]? ignoreProperties) => CopyFrom<T>(target, source, ignoreProperties);

  public static T CopyFrom<T>(this T target, object source) => CopyFrom<T>(target, source, null);
  public static T CopyFrom<T>(this T target, object source, string[]? ignoreProperties)
  {
    if (target == null) return target;
    ignoreProperties ??= Array.Empty<string>();
    var propsSource = source.GetType().GetProperties().Where(x => x.CanRead && !ignoreProperties.Contains(x.Name));
    var propsTarget = target.GetType().GetProperties().Where(x => x.CanWrite);

    propsTarget
    .Where(prop => propsSource.Any(x => x.Name == prop.Name))
    .ToList()
    .ForEach(prop =>
    {
      var propSource = propsSource.Where(x => x.Name == prop.Name).First();
      prop.SetValue(target, propSource.GetValue(source));
    });
    return target;
  }
  
  public static T TransformTo<T>(this object source)
  {
    var propsSource = source.GetType().GetProperties().Where(x => x.CanRead);
    var propsTarget = typeof(T).GetConstructors().First().GetParameters();

    object?[] parameterValues = propsTarget
       .Select(prop => propsSource.Any(x => x.Name == prop.Name)
          ? propsSource.Where(x => x.Name == prop.Name).First().GetValue(source)
          : null)
       .ToArray();
    return (T)Activator.CreateInstance(typeof(T), parameterValues)!;
  }

  public static void Log(this ControllerBase controller, string msg = "", [CallerMemberName] string callerMethod = "")
  {
    //Note: Color output requires "launchBrowser": false in launchSettings.json
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write($"{DateTime.Now:HH:mm:ss.ff}");
    Console.BackgroundColor = ConsoleColor.Black;
    string method = controller.Request.HttpContext.Request.Method;
    Console.ForegroundColor = method == "GET" ? ConsoleColor.Green : method == "DELETE" ? ConsoleColor.Red : ConsoleColor.Cyan;
    Console.Write($" {controller.Request.HttpContext.Request.Method} ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"{controller.Request.HttpContext.Request.Path}");
    if ($"{controller.Request.QueryString}".Length>1) Console.Write($"{controller.Request.QueryString} ");
    else Console.Write(" ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"{callerMethod} {msg}");
    Console.ResetColor();
  }

  public static void Log(this Microsoft.AspNetCore.SignalR.Hub hub, string msg = "", [CallerMemberName] string callerMethod = "")
  {
    //Note: Color output requires "launchBrowser": false in launchSettings.json
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write($"{DateTime.Now:HH:mm:ss.ff}");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($" {hub.Context.ConnectionId} ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"{hub.GetType().Name}.{callerMethod} {msg}");
    Console.ResetColor();
  }
}

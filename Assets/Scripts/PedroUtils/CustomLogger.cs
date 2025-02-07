// maded by Pedro M Marangon
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PedroUtils
{
	public static class CustomLogger
	{
		private static string ERROR_PREFIX => "<!>".Color("#ff1748");
		private static string WARN_PREFIX => "⚠".Color("yellow");
		private static string SUCESS_PREFIX => "<✔>".Color("#40ff37");

		private static void DoLog(Action<string, Object> logFunction, string prefix, Object myObj, params object[] msg)
		{
#if UNITY_EDITOR
			logFunction($"{prefix}[{myObj.name.Color("lightblue")}]: { string.Join(";", msg) }\n ", myObj);
#endif
		}
		public static void Log(this Object myObj, params object[] msg) => DoLog(Debug.Log, "", myObj, msg);
		public static void LogError(this Object myObj, params object[] msg) => DoLog(Debug.LogError, WARN_PREFIX, myObj, msg);
		public static void LogWarning(this Object myObj, params object[] msg) => DoLog(Debug.LogWarning, ERROR_PREFIX, myObj, msg);
		public static void LogSucess(this Object myObj, params object[] msg) => DoLog(Debug.Log, SUCESS_PREFIX, myObj, msg);

		private static void DoLog(Action<string> logFunction, string prefix, params object[] msg)
		{
#if UNITY_EDITOR
			logFunction($"{prefix}: { string.Join(";", msg) }\n ");
#endif
		}
		public static void Log(params object[] msg) => DoLog(Debug.Log, "", msg);
		public static void LogError(params object[] msg) => DoLog(Debug.LogError, ERROR_PREFIX, msg);
		public static void LogWarning(params object[] msg) => DoLog(Debug.LogWarning, WARN_PREFIX, msg);
		public static void LogSucess(params object[] msg) => DoLog(Debug.Log, SUCESS_PREFIX, msg);
	}
}
using Assets.Scripts.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class ApiManager
		: MonoBehaviour
	{
		private const string ENDPOINT = "http://localhost:35218";

		private static Dictionary<string, WWW> activeCalls = new Dictionary<string, WWW>();

		public static bool LoggingEnabled { get; set; }

		public static class UserCalls
		{
			public static IEnumerator GetUser(Guid id, Action<User> action)
			{
				var call = Call("User/Get", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				Log(call.text);

				var user = JsonConvert.DeserializeObject<User>(call.text);
				action(user);
			}
		}

		private static WWW Call(string route, IDictionary<string, string> queryParams)
		{
			RemoveDeactiveCalls();

			string uri = GetUri(route, queryParams);
			if (activeCalls.ContainsKey(uri))
			{
				throw new InvalidOperationException(String.Format("Multiple calls were requested for URI '{0}'", uri));
			}
			return activeCalls[uri] = new WWW(uri);
		}

		private static void RemoveDeactiveCalls()
		{
			foreach (var deactiveCall in activeCalls.Where(x => x.Value.isDone))
			{
				activeCalls.Remove(deactiveCall.Key);
			}
		}

		private static string GetUri(string route, IDictionary<string, string> queryParams)
		{
			var sb = new StringBuilder(String.Format("{0}/{1}?", ENDPOINT.TrimEnd('?', '/'), route));
			if (queryParams != null)
			{
				queryParams.Aggregate(sb, (_, param) => sb.AppendFormat("{0}={1}&", param.Key, param.Value));
			}

			return sb.ToString().TrimEnd('&');
		}

		private static void Log(string message)
		{
			if (LoggingEnabled)
			{
				Debug.Log(message);
			}
		}
	}
}

using Assets.Scripts.Enumerations;
using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public static class ApiManager
	{
		private const string ENDPOINT = "http://glamd.mikevdongen.nl/api";

		private static Dictionary<string, WWW> activeCalls = new Dictionary<string, WWW>();

		public static LogLevel LogLevel { get; set; }

		public static class MatchCalls
		{
			public static IEnumerator Create(int seed, Guid opponentId, Guid creatorId, int creatorScore, Action<Match> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Match/Create", new Dictionary<string, string>
				{
					{ "seed", seed.ToString() },
					{ "opponentId", opponentId.ToString() },
					{ "creatorId", creatorId.ToString() },
					{ "creatorScore", creatorScore.ToString() },
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator Update(Guid id, int opponentScore, Action<Match> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Match/Update", new Dictionary<string, string>
				{
					{ "id", id.ToString() },
					{ "opponentScore", opponentScore.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}
		}

		public static class FriendCalls
		{
			public static IEnumerator Invite(Guid userId, Guid invitedId, Action<Friend> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Friend/Invite", new Dictionary<string, string>
				{
					{ "userId", userId.ToString() },
					{ "invitedId", invitedId.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator Accept(Guid id, Action<Friend> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Friend/Accept", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}
		}

		public static class ReplayCalls
		{
			public static IEnumerator GetReplay(Guid id, Action<string> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Replay/Get", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator CreateReplay(Guid matchId, string data, Action<Guid> onSuccess, Action<Error> onFailure)
			{
				var call = Call("Replay/Get", new Dictionary<string, string>
				{
					{ "matchId", matchId.ToString() },
					{ "data", data }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}
		}

		public static class UserCalls
		{
			public static IEnumerator LoginUser(string username, string password, Action<User> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Login", new Dictionary<string, string>
				{
					{ "username", username },
					{ "password", Hasher.Hash(password) }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator GetUser(Guid id, Action<User> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Get", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator CreateUser(string username, string password, Action<User> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Create", new Dictionary<string, string>
				{
					{ "username", username },
					{ "password", Hasher.Hash(password) }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator UserExists(string username, Action<bool> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Exists", new Dictionary<string, string>
				{
					{ "username", username }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator UserFriends(Guid id, Action<Friend[]> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Friends", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator UserInvites(Guid id, Action<Friend[]> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Invites", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
			}

			public static IEnumerator UserMatches(Guid id, Action<Match[]> onSuccess, Action<Error> onFailure)
			{
				var call = Call("User/Matches", new Dictionary<string, string>
				{
					{ "id", id.ToString() }
				});

				yield return call;
				HandleFinishedCall(call, onSuccess, onFailure);
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

		private static void HandleFinishedCall<T>(WWW call, Action<T> onSuccess, Action<Error> onFailure)
		{
			Log(call);

			var statusCode = GetStatusCode(call);
			if (statusCode == HttpStatusCode.OK)
			{
				onSuccess(JsonConvert.DeserializeObject<T>(call.text));
			}
			else if (statusCode == HttpStatusCode.ServiceUnavailable)
			{
				onFailure(new Error
				{
					Message = "The server is currently unavailable."
				});
			}
			else
			{
				onFailure(JsonConvert.DeserializeObject<Error>(call.text));
			}
		}

		private static HttpStatusCode GetStatusCode(WWW call)
		{
			if (!call.responseHeaders.ContainsKey("STATUS"))
			{
				return HttpStatusCode.ServiceUnavailable;
			}

			var statusSegments = call.responseHeaders["STATUS"].Split(' ');
			return (HttpStatusCode)Int32.Parse(statusSegments[(int)HttpStatusCodeSegment.Code]);
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

		private static void Log(WWW www)
		{
			if (LogLevel == LogLevel.Basic)
			{
				Debug.LogFormat("API Call made for URI: {0}", www.url);
			}

			if (LogLevel == LogLevel.Full)
			{
				foreach (var header in www.responseHeaders)
				{
					Debug.LogFormat("Response Header: {0} - {1}", header.Key, header.Value);
				}

				Debug.LogFormat("Response: {0}", www.text);
			}
		}
	}
}

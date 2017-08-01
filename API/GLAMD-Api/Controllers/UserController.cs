using GLAMD_Api.Models;
using GLAMD_Api.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GLAMD_Api.Controllers
{
	public class UserController
		: BaseController
	{
		[HttpGet]
		public async Task<ActionResult> Get(Guid id) => await Get(id, db => db.Users, user => new UserViewModel(user)).ConfigureAwait(false);

		[HttpGet]
		public ActionResult Friends(Guid id)
		{
			using (var db = new GLAMD_DbContext())
			{
				if (!db.Users.Any(x => x.Id == id))
				{
					return NotFound(nameof(User), id);
				}

				var friends = db.Friends.Where(x => x.Accepted && (x.User.Id == id ^ x.Invited.Id == id)).ToArray();
				return Json(friends.Select(friend => new FriendViewModel(friend)).ToArray());
			}
		}

		[HttpGet]
		public ActionResult Matches(Guid id)
		{
			using (var db = new GLAMD_DbContext())
			{
				if (!db.Users.Any(x => x.Id == id))
				{
					return NotFound(nameof(User), id);
				}

				var matches = db.Matches.Where(x => x.Creator.Id == id || x.Opponent.Id == id).ToArray();
				return Json(matches.Select(friend => new MatchViewModel(friend)).ToArray());
			}
		}

		[HttpGet]
		public ActionResult Invites(Guid id)
		{
			using (var db = new GLAMD_DbContext())
			{
				if (!db.Users.Any(x => x.Id == id))
				{
					return NotFound(nameof(User), id);
				}

				var friends = db.Friends.Where(x => !x.Accepted && x.Invited.Id == id).ToArray();
				return Json(friends.Select(friend => new FriendViewModel(friend)).ToArray());
			}
		}

		[HttpGet]
		public async Task<ActionResult> Create(string username, string password)
		{
			if (username == null)
			{
				return NullError(nameof(username));
			}
			else if (password == null)
			{
				return NullError(nameof(password));
			}
			else if (username.Length < Models.User.USERNAME_MINIMUM_LENGTH || username.Length > Models.User.USERNAME_MAXIMUM_LENGTH)
			{
				return BadRequest($"{nameof(username)} must be between {Models.User.USERNAME_MINIMUM_LENGTH} and {Models.User.USERNAME_MAXIMUM_LENGTH} characters long.");
			}
			else if (password.Length != Models.User.PASSWORD_LENGTH)
			{
				return BadRequest($"{nameof(password)} must be exactly {Models.User.PASSWORD_LENGTH} characters long.");
			}

			using (var db = new GLAMD_DbContext())
			{
				if (db.Users.Any(x => x.Username == username))
				{
					return Conflict($"{nameof(Models.User)} already exists.");
				}

				var user = db.Users.Add(new User
				{
					Username = username,
					Password = password
				});
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(new UserViewModel(user));
			}
		}

		[HttpGet]
		public async Task<ActionResult> Delete(Guid id) => await Delete(id, db => db.Users).ConfigureAwait(false);
	}
}

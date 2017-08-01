using GLAMD_Api.Models;
using GLAMD_Api.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GLAMD_Api.Controllers
{
	public class FriendController
		: BaseController
	{
		[HttpGet]
		public async Task<ActionResult> Get(Guid id) => await Get(id, db => db.Friends, friend => new FriendViewModel(friend)).ConfigureAwait(false);

		[HttpGet]
		public async Task<ActionResult> Invite(Guid userId, Guid invitedId)
		{
			using (var db = new GLAMD_DbContext())
			{
				if (userId == invitedId)
				{
					return BadRequest("User and invited user are the same user.");
				}
				else if (!db.Users.Any(x => x.Id == userId))
				{
					return NotFound(nameof(User), userId);
				}
				else if (!db.Users.Any(x => x.Id == invitedId))
				{
					return NotFound(nameof(User), invitedId);
				}

				var existingRequest = db.Friends.FirstOrDefault(x => (x.User.Id == userId && x.Invited.Id == invitedId) || (x.User.Id == invitedId && x.Invited.Id == userId));
				if (existingRequest != null)
				{
					if (existingRequest.Accepted)
					{
						return BadRequest("User is already friends with invited user.");
					}
					else
					{
						return BadRequest("Friend request is pending.");
					}
				}

				var friend = db.Friends.Add(new Friend
				{
					User = db.Users.Find(userId),
					Invited = db.Users.Find(invitedId),
					Accepted = false
				});
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(new FriendViewModel(friend));
			}
		}

		[HttpGet]
		public async Task<ActionResult> Accept(Guid id)
		{
			using (var db = new GLAMD_DbContext())
			{
				var friend = await db.Friends.FindAsync(id).ConfigureAwait(false);
				if (friend == null)
				{
					return NotFound(nameof(Friend), id);
				}
				else if (friend.Accepted)
				{
					return BadRequest("Friend invite is already accepted.");
				}

				friend.Accepted = true;
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(new FriendViewModel(friend));
			}
		}

		[HttpGet]
		public async Task<ActionResult> Delete(Guid id) => await Delete(id, db => db.Friends).ConfigureAwait(false);
	}
}
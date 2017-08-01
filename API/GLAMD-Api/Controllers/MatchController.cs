using GLAMD_Api.Models;
using GLAMD_Api.Models.Enumerations;
using GLAMD_Api.Models.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GLAMD_Api.Controllers
{
	public class MatchController
		: BaseController
    {
		[HttpGet]
		public async Task<ActionResult> Get(Guid id) => await Get(id, db => db.Matches, user => new MatchViewModel(user)).ConfigureAwait(false);

		[HttpGet]
		public async Task<ActionResult> Create(int seed, Guid opponentId, Guid creatorId, int creatorScore)
		{
			using (var db = new GLAMD_DbContext())
			{
				var creator = await db.Users.FindAsync(creatorId).ConfigureAwait(false);
				if (creator == null)
				{
					return NotFound(nameof(Models.User), creatorId);
				}

				var opponent = await db.Users.FindAsync(opponentId).ConfigureAwait(false);
				if (opponent == null)
				{
					return NotFound(nameof(Models.User), opponentId);
				}

				var match = db.Matches.Add(new Match
				{
					Seed = seed,
					Opponent = opponent,
					Creator = creator,
					CreatorScore = creatorScore,
					Status = Status.Pending
				});
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(new MatchViewModel(match));
			}
		}

		[HttpGet]
		public async Task<ActionResult> Update(Guid id, int opponentScore)
		{
			using (var db = new GLAMD_DbContext())
			{
				var match = await db.Matches.FindAsync(id).ConfigureAwait(false);
				if (match == null)
				{
					return NotFound(nameof(Match), id);
				}
				else if (match.Status == Status.Pending)
				{
					return BadRequest($"{nameof(Match)} is still pending.");
				}
				else if (match.Status == Status.Finished)
				{
					return Conflict($"{nameof(Match)} is already finished.");
				}

				match.OpponentScore = opponentScore;
				match.Status = Status.Finished;
				match.Victor = DetermineVictor(match);
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(new MatchViewModel(match));
			}
		}

		[HttpGet]
		public async Task<ActionResult> Delete(Guid id) => await Delete(id, db => db.Matches).ConfigureAwait(false);

		private User DetermineVictor(Match match)
		{
			if (match.CreatorScore > match.OpponentScore)
			{
				return match.Creator;
			}
			else if (match.OpponentScore > match.CreatorScore)
			{
				return match.Opponent;
			}

			return null;
		}
	}
}
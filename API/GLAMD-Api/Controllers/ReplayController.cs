using GLAMD_Api.Models;
using GLAMD_Api.Models.Enumerations;
using GLAMD_Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GLAMD_Api.Controllers
{
    public class ReplayController
		: BaseController
    {
		[HttpGet]
		public async Task<ActionResult> Get(Guid id) => await Get(id, db => db.Replays, replay => replay.Data);

		[HttpGet]
		public async Task<ActionResult> Create(Guid matchId, string data)
		{
			if (String.IsNullOrWhiteSpace(data))
			{
				return NullError(nameof(data));
			}

			using (var db = new GLAMD_DbContext())
			{
				var match = await db.Matches.FindAsync(matchId);
				if (match == null)
				{
					return BadRequest($"{nameof(Match)} does not exist.");
				}
				else if (match.Replay != null)
				{
					return Conflict($"{nameof(Replay)} already exists for match id '{matchId}'.");
				}

				var replay = db.Replays.Add(new Replay
				{
					Data = data
				});
				match.Replay = replay;
				match.Status = Status.Open;
				await db.SaveChangesAsync().ConfigureAwait(false);

				return Json(replay.Id);
			}
		}

		[HttpGet]
		public async Task<ActionResult> Delete(Guid id) => await Delete(id, db => db.Replays);
	}
}
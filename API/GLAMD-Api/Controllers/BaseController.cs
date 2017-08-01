using GLAMD_Api.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GLAMD_Api.Controllers
{
	public abstract class BaseController
		: Controller
	{
		public async Task<ActionResult> Get<TEntity>(Guid id, Func<GLAMD_DbContext, DbSet<TEntity>> tableSelector)
			where TEntity
			: Entity
			=> await Get<TEntity, object>(id, tableSelector, null);

		public async Task<ActionResult> Get<TEntity, TViewModel>(Guid id, Func<GLAMD_DbContext, DbSet<TEntity>> tableSelector, Func<TEntity, TViewModel> viewModelConverter)
			where TEntity
			: Entity
		{
			using (var db = new GLAMD_DbContext())
			{
				var entity = await tableSelector(db).FindAsync(id);

				if (entity == null)
				{
					return NotFound(typeof(TEntity).Name, id);	
				}

				if (viewModelConverter == null)
				{
					return Json(entity);
				}

				return Json(viewModelConverter(entity));
			}
		}

		public async Task<ActionResult> Delete<TEntity>(Guid id, Func<GLAMD_DbContext, DbSet<TEntity>> tableSelector)
			where TEntity
			: Entity
		{
			using (var db = new GLAMD_DbContext())
			{
				var entity = await tableSelector(db).FindAsync(id);

				if (entity == null)
				{
					return NotFound(typeof(TEntity).Name, id);
				}

				tableSelector(db).Remove(entity);
				await db.SaveChangesAsync();

				return Ok($"{typeof(TEntity).Name} successfully deleted.");
			}
		}

		public new JsonResult Json(object data) => Json(data, JsonRequestBehavior.AllowGet);

		public HttpStatusCodeResult Ok(string description) => StatusCode(HttpStatusCode.OK, description);

		public HttpStatusCodeResult NullError(string name) => BadRequest($"{name} may not be null.");

		public HttpStatusCodeResult BadRequest(string description) => StatusCode(HttpStatusCode.BadRequest, description);

		public HttpStatusCodeResult NotFound(string typeName, Guid id) => HttpNotFound($"Could not find {typeName} for id '{id}'.");

		public HttpStatusCodeResult Conflict(string description) => StatusCode(HttpStatusCode.Conflict, description);

		public HttpStatusCodeResult StatusCode(HttpStatusCode code, string description) => new HttpStatusCodeResult(code, description);
	}
}
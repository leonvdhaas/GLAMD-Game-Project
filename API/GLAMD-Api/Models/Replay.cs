using System.ComponentModel.DataAnnotations;

namespace GLAMD_Api.Models
{
	public class Replay
		: Entity
	{
		[Required(AllowEmptyStrings = false)]
		public string Data { get; set; }
	}
}
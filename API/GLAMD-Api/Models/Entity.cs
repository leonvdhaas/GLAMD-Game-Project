using System;
using System.ComponentModel.DataAnnotations;

namespace GLAMD_Api.Models
{
	public abstract class Entity
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
	}
}
using GLAMD_Api.Models;
using System.Data.Entity;

namespace GLAMD_Api
{
	public partial class GLAMD_DbContext
		: DbContext
	{
		public GLAMD_DbContext()
			: base("name=glamddb")
		{
		}

		public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<Friend> Friends { get; set; }

		public virtual DbSet<Match> Matches { get; set; }

		public virtual DbSet<Replay> Replays { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Friend>()
				.HasRequired(f => f.Invited)
				.WithMany()
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Match>()
				.HasRequired(m => m.Creator)
				.WithMany()
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Match>()
				.HasRequired(m => m.Opponent)
				.WithMany()
				.WillCascadeOnDelete(false);
		}
	}
}

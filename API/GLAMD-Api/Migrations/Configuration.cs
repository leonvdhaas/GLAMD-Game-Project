using System.Data.Entity.Migrations;

namespace GLAMD_Api.Migrations
{
	internal sealed class Configuration
		: DbMigrationsConfiguration<GLAMD_DbContext>
    {
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(GLAMD_DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

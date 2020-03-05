#if NET452
using System.Data.Entity.Migrations;

namespace Headstone.Framework.Models.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Headstone.Framework.Models.Contexts.FrameworkDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Headstone.Framework.Models.Contexts.FrameworkDbContext context)
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
#endif
#if NET452
using System.Data.Entity.Migrations;

namespace Headstone.Framework.SaaS.Models.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SaasDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SaasDbContext context)
        {
            
        }
    }
}
#endif
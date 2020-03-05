using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Logging;
#if NET452
using System.Data.Entity;
using Headstone.Framework.Models.Session;
#elif NETCOREAPP2_2
using Microsoft.EntityFrameworkCore;
#endif

namespace Headstone.Framework.Models.Contexts
{
    public class FrameworkDbContext : DbContext
    {
#if NET452
		public FrameworkDbContext() : base("name=MAIN")
		{

			// Set the initializer
			Database.SetInitializer<FrameworkDbContext>(null);
			//Database.SetInitializer(new CoreDbContextInitializer());
		}
#elif NETCOREAPP2_2
        public FrameworkDbContext()
        {
        }
#endif


        public static FrameworkDbContext Create()
        {
            return new FrameworkDbContext();
        }

#if NET452
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        #region [ Do not map complex data types ]

            // SessionStateItem
            modelBuilder.Types<SessionStateItem>().Configure(c => c.Ignore(i => i.Items));
            modelBuilder.Types<SessionStateItem>().Configure(c => c.Ignore(i => i.StaticObjects));

        #endregion
        }
#endif

        // CONFIG
        public DbSet<ConfigRecord> SystemConfiguration { get; set; }

#if NET452
        // SESSION
        public DbSet<SessionStateItem> Sessions { get; set; }
#endif
        // LOGGING
        public DbSet<LogRecord> Logs { get; set; }

    }
}

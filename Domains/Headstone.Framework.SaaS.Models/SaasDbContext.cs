using Headstone.Framework.SaaS.Models.Identity;
#if NET452
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endif

namespace Headstone.Framework.SaaS.Models
{
#if NET452
    public class SaasDbContext : IdentityDbContext<HeadstoneUser, HeadstoneRole, int, HeadstoneUserLogin, HeadstoneUserRole, HeadstoneUserClaim>
    {
        public SaasDbContext() : base("name=MAIN") { }

        public static SaasDbContext Create()
        {
            return new SaasDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// Disable model check
            //Database.SetInitializer<SaasDbContext>(null);

            //base.OnModelCreating(modelBuilder);

    #region [ Identity ]

            modelBuilder.Entity<HeadstoneUserLogin>().Map(c =>
            {
                c.ToTable("UserLogins");
                c.Properties(p => new
                {
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });

            // Mapping for ApiRole
            modelBuilder.Entity<HeadstoneRole>().Map(c =>
            {
                c.ToTable("Roles");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                    p.Name
                });
            }).HasKey(p => p.Id);
            modelBuilder.Entity<HeadstoneRole>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<HeadstoneUser>().Map(c =>
            {
                c.ToTable("Users");
                c.Property(p => p.Id).HasColumnName("UserId");
                c.Properties(p => new
                {
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.MobileNumber,
                    p.MobileNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.UserName,
                    p.Firstname,
                    p.Lastname,
                    p.Gender,
                    p.Birthdate
                });
            }).HasKey(c => c.Id);
            modelBuilder.Entity<HeadstoneUser>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<HeadstoneUser>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<HeadstoneUser>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);

            modelBuilder.Entity<HeadstoneUserRole>().Map(c =>
            {
                c.ToTable("UserRoles");
                c.Properties(p => new
                {
                    p.UserId,
                    p.RoleId,
                    p.TenantId
                });
            })
            .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<HeadstoneUserClaim>().Map(c =>
            {
                c.ToTable("UserClaims");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType
                });
            }).HasKey(c => c.Id);

    #endregion
        }
#elif NETCOREAPP2_2
    public class SaasDbContext : IdentityDbContext<HeadstoneUser, HeadstoneRole, int, HeadstoneUserClaim, HeadstoneUserRole, HeadstoneUserLogin, HeadstoneRoleClaim, IdentityUserToken<int>>
    {
        public SaasDbContext()
        {
        }

        public SaasDbContext(DbContextOptions options) : base(options)
        {
        }

        public static SaasDbContext Create()
        {
            return new SaasDbContext();
        }

#endif
        public DbSet<Tenant> Tenants { get; set; }


        public DbSet<Application> Applications { get; set; }

#pragma warning disable CS0108 // 'SaasDbContext.UserRoles' hides inherited member 'IdentityDbContext<HeadstoneUser, HeadstoneRole, int, HeadstoneUserClaim, HeadstoneUserRole, HeadstoneUserLogin, HeadstoneRoleClaim, IdentityUserToken<int>>.UserRoles'. Use the new keyword if hiding was intended.
        public DbSet<HeadstoneUserRole> UserRoles { get; set; }
#pragma warning restore CS0108 // 'SaasDbContext.UserRoles' hides inherited member 'IdentityDbContext<HeadstoneUser, HeadstoneRole, int, HeadstoneUserClaim, HeadstoneUserRole, HeadstoneUserLogin, HeadstoneRoleClaim, IdentityUserToken<int>>.UserRoles'. Use the new keyword if hiding was intended.

        public DbSet<Package> Packages { get; set; }
    }
}

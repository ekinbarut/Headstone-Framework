using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Identity;
using System;
#if NET452
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
#endif

namespace Headstone.Framework.SaaS.Identity
{
#if NET452
    public class HeadstoneUserManager : UserManager<HeadstoneUser, int>
    {
        public HeadstoneUserManager(IUserStore<HeadstoneUser, int> store) : base(store)
        {
        }

        public static HeadstoneUserManager Create(IdentityFactoryOptions<HeadstoneUserManager> options, IOwinContext context)
        {
            var manager = new HeadstoneUserManager(new HeadstoneUserStore(context.Get<SaasDbContext>()));

            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<HeadstoneUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            
            // Register two factor authentication providers. This application uses Phone 
            // and Emails as a step of receiving a code for verifying the user 
            // You can write your own provider and plug in here. 
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<HeadstoneUser, int>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<HeadstoneUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<HeadstoneUser, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public string GetUserGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
#elif NETCOREAPP2_2
    public class HeadstoneUserManager : UserManager<HeadstoneUser>
    {
        public HeadstoneUserManager(IUserStore<HeadstoneUser> store) : base(store, null, null, null, null, null, null, null, null)
        {
        }


        public string GetUserGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
#endif
}

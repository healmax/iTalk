using iTalk.DAO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace iTalk.API {
    /// <summary>
    /// User Manager
    /// </summary>
    public class UserManager : UserManager<iTalkUser, long> {
        /// <summary>
        /// 建構函數
        /// </summary>
        UserManager()
            : base(new UserStore<iTalkUser, iTalkRole, long, iTalkUserLogin, iTalkUserRole, iTalkUserClaim>(new iTalkDbContext())) { }

        /// <summary>
        /// 建立User Manager委派，定義了帳密的格式與Token Provider
        /// </summary>
        /// <param name="options">IdentityFactoryOptions</param>
        /// <param name="context">IOwinContext</param>
        /// <returns></returns>
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context) {
            var manager = new UserManager();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<iTalkUser, long>(manager) {
                AllowOnlyAlphanumericUserNames = true,
                //RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 3,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null) {
                manager.UserTokenProvider = new DataProtectorTokenProvider<iTalkUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}

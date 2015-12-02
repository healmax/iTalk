using Microsoft.AspNet.Identity.EntityFramework;

namespace iTalk.DAO {
    /// <summary>
    /// iTalk User Claim
    /// </summary>
    public class iTalkUserClaim : IdentityUserClaim<long> { }

    /// <summary>
    /// iTalk User Login
    /// </summary>
    public class iTalkUserLogin : IdentityUserLogin<long> { }

    /// <summary>
    /// iTalk User Role
    /// </summary>
    public class iTalkRole : IdentityRole<long, iTalkUserRole> { }

    /// <summary>
    /// iTalk User Role
    /// </summary>
    public class iTalkUserRole : IdentityUserRole<long> { }
}

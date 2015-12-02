using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace iTalk.API {
    /// <summary>
    /// 自訂 SignalR Hub User Id Provider
    /// </summary>
    public class UserIdProvider : IUserIdProvider {
        /// <summary>
        /// 回傳 User Id
        /// </summary>
        /// <param name="request">請求</param>
        /// <returns>User Id</returns>
        public string GetUserId(IRequest request) {
            return request.User.Identity.GetUserId<long>().ToString();
        }
    }
}
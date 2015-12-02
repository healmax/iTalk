using iTalk.API.Controllers;
using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API {
    /// <summary>
    /// 控制器擴充方法
    /// </summary>
    public static class ControllerExtensions {
        /// <summary>
        /// 檢查群組關係
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="relationshipId">群組 Id</param>
        /// <param name="throwIfNotExist">指定之群組不存在時是否拋出例外</param>
        /// <param name="throwIfNoRelationship">不是群組成員時是否拋出例外</param>
        /// <returns>朋友關聯狀態</returns>
        public static async Task<RelationCheck> ValidateGroup(this DefaultApiController controller, long relationshipId, bool throwIfNotExist = true, bool throwIfNoRelationship = true) {
            iTalkDbContext dbContext = controller.Request.GetOwinContext().Get<iTalkDbContext>();
            Group group = await dbContext.Groups.FindAsync(relationshipId);

            if (group == null) {
                if (throwIfNotExist) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.Group, relationshipId, Resources.NotExist);
                }
                else {
                    return RelationCheck.NotExist;
                }
            }

            bool isMember = await dbContext.Entry(group)
                .Collection(g => g.Members)
                .Query()
                .AnyAsync(m => m.UserId == controller.UserId);

            if (!isMember && throwIfNoRelationship) {
                throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0}{1} {2} {3}{4}",
                    Resources.You, Resources.Not, relationshipId, Resources.Group, Resources.Member);
            }

            return isMember ? RelationCheck.In : RelationCheck.Not;
        }

        /// <summary>
        /// 檢查朋友關係
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="friendId">朋友 Id</param>
        /// <param name="throwIfNotExist">指定之使用者不存在時是否拋出例外</param>
        /// <param name="throwIfNotFriend">指定之使用者不是朋友時是否拋出例外</param>
        /// <returns>朋友關聯狀態</returns>
        public static async Task<RelationCheck> ValidateFriendship(this DefaultApiController controller, long friendId, bool throwIfNotExist = true, bool throwIfNotFriend = true) {
            iTalkDbContext dbContext = controller.Request.GetOwinContext().Get<iTalkDbContext>();
            bool userExist = await dbContext.Users.AnyAsync(u => u.Id == friendId);

            if (!userExist) {
                if (throwIfNotExist) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.User, friendId, Resources.NotExist);
                }
                else {
                    return RelationCheck.NotExist;
                }
            }

            long id = controller.UserId; ;

            bool isFriend = await dbContext.Friendships
                .AnyAsync(rs => (rs.UserId == id && rs.InviteeId == friendId) ||
                    (rs.UserId == friendId && rs.InviteeId == id));

            if (!isFriend) {
                if (throwIfNotFriend) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1}", friendId, Resources.NotYourFriend);
                }
                else {
                    return RelationCheck.Not;
                }
            }

            return RelationCheck.In;
        }

        /// <summary>
        /// 建立回應例外
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="code">Http狀態碼</param>
        /// <param name="format">訊息內容</param>
        /// <param name="args">參數</param>
        /// <returns>回應例外</returns>
        public static HttpResponseException CreateResponseException(this DefaultApiController controller, HttpStatusCode code, string format, params object[] args) {
            var response = controller.Request.CreateResponse(code, new ExecuteResult(false, (int)code, string.Format(format, args)));
            return new HttpResponseException(response);
        }

        /// <summary>
        /// 取得 Model 上的錯誤訊息
        /// </summary>
        /// <returns></returns>
        public static string GetError(this DefaultApiController controller) {
            StringBuilder errors = new StringBuilder();

            foreach (var pair in controller.ModelState) {
                errors.AppendFormat("{0} : {1}", pair.Key, string.Join(",", pair.Value.Errors.Select(e => e.ErrorMessage)));
                errors.AppendLine();
            }

            return errors.ToString();
        }
    }
}
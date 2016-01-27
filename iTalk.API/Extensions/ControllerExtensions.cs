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
        /// <param name="groupId">群組 Id</param>
        /// <param name="throwIfNotExist">指定之群組不存在時是否拋出例外</param>
        /// <param name="throwIfNoRelationship">不是群組成員時是否拋出例外</param>
        /// <returns>朋友關聯狀態</returns>
        public static async Task<ValidateResult<Group>> ValidateGroup(this DefaultApiController controller, long groupId, bool throwIfNotExist = true, bool throwIfNoRelationship = true) {
            iTalkDbContext dbContext = controller.Request.GetOwinContext().Get<iTalkDbContext>();
            Group group = await dbContext.Groups.FindAsync(groupId);
            //bool exist = await dbContext.Groups.AnyAsync(g => g.Id == groupId);

            if (group == null) {
                if (throwIfNotExist) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.Group, groupId, Resources.NotExist);
                }
                else {
                    return new ValidateResult<Group>(null, RelationCheck.NotExist);
                }
            }

            bool isMember = await dbContext.Entry(group)
                .Collection(g => g.Members)
                .Query()
                .AnyAsync(m => m.UserId == controller.UserId);
            //Group group = await dbContext.GroupMembers
            //    .Where(gm => gm.UserId == controller.UserId)
            //    .Select(gm => gm.Group)
            //    .FirstOrDefaultAsync(g => g.Id == groupId);

            if (!isMember && throwIfNoRelationship) {
                throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0}{1} {2} {3}{4}",
                    Resources.You, Resources.Not, groupId, Resources.Group, Resources.Member);
            }

            return isMember ? new ValidateResult<Group>(group, RelationCheck.HasRelation) :
                new ValidateResult<Group>(null, RelationCheck.NoRelation);
        }

        /// <summary>
        /// 檢查朋友關係
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="friendId">朋友 Id</param>
        /// <param name="throwIfNotExist">指定之使用者不存在時是否拋出例外</param>
        /// <param name="throwIfNotFriend">指定之使用者不是朋友時是否拋出例外</param>
        /// <returns>朋友關聯狀態</returns>
        public static async Task<ValidateResult<Friendship>> ValidateFriendship(this DefaultApiController controller, long friendId, bool throwIfNotExist = true, bool throwIfNotFriend = true) {
            iTalkDbContext dbContext = controller.Request.GetOwinContext().Get<iTalkDbContext>();
            bool userExist = await dbContext.Users.AnyAsync(u => u.Id == friendId);

            if (!userExist) {
                if (throwIfNotExist) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.User, friendId, Resources.NotExist);
                }
                else {
                    return new ValidateResult<Friendship>(null, RelationCheck.NotExist);
                }
            }

            long id = controller.UserId; ;

            Friendship fs = await dbContext.Friendships
                .FirstOrDefaultAsync(rs => (rs.UserId == id && rs.InviteeId == friendId) ||
                    (rs.UserId == friendId && rs.InviteeId == id));

            if (fs == null) {
                if (throwIfNotFriend) {
                    throw controller.CreateResponseException(HttpStatusCode.NotFound, "{0} {1}", friendId, Resources.NotYourFriend);
                }
                else {
                    return new ValidateResult<Friendship>(null, RelationCheck.NoRelation);
                }
            }

            return new ValidateResult<Friendship>(fs, RelationCheck.HasRelation);
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

    /// <summary>
    /// Relation Validation Result
    /// </summary>
    /// <typeparam name="T">Validate Relation Type</typeparam>
    public class ValidateResult<T> {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="target">Validation Target</param>
        /// <param name="check">RelationCheck</param>
        public ValidateResult(T target, RelationCheck check) {
            this.Target = target;
            this.Check = check;
        }

        /// <summary>
        /// 取得 Validation Targe
        /// </summary>
        public T Target { get; private set; }

        /// <summary>
        /// 取得 RelationCheck
        /// </summary>
        public RelationCheck Check { get; private set; }
    }
}
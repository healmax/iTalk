using iTalk.API.Controllers;
using iTalk.API.Models;
using iTalk.DAO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace iTalk.API {
    /// <summary>
    /// 新增使用者時，自動將測試用者者加為好友
    /// </summary>
    public class DefaultTestFriendFilterAttribute : ActionFilterAttribute {
        /// <summary>
        /// Action 執行後
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken) {
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            if (actionExecutedContext.Exception != null) {
                return;
            }

            AccountViewModel account = actionExecutedContext.ActionContext.ActionArguments.Values
                .OfType<AccountViewModel>()
                .FirstOrDefault();

            if (account != null) {
                var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as DefaultApiController;
                string[] testUserNames = { "TestUser1", "TestUser2" };
                var testUserIds = await controller.DbContext.Users
                    .Where(u => testUserNames.Contains(u.UserName))
                    .Select(u => u.Id)
                    .ToArrayAsync();

                string upperUserName = account.UserName.ToUpper();
                var currentUserId = await controller.DbContext.Users
                    .Where(u => u.UserName.ToUpper() == upperUserName)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                DateTime date = DateTime.UtcNow;

                foreach (long id in testUserIds) {
                    controller.DbContext.Friendships.AddRange(new[] {
                    new Friendship(currentUserId, id, RelationshipStatus.Accepted, date, date),
                    new Friendship(id, currentUserId, RelationshipStatus.Accepted, date, date),
                });
                }

                await controller.DbContext.SaveChangesAsync();
            }
        }
    }
}
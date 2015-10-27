using iTalk.API.Models;
using iTalk.DAO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTalk.API.Areas.Admin.Controllers {
    /// <summary>
    /// 後台帳戶管理控制器
    /// </summary>
    public class AccountController : Controller {
        /// <summary>
        /// 列出所有使用者
        /// </summary>
        /// <returns>使用者列表頁面</returns>
        public async Task<ActionResult> Index() {
            return this.View(await Task.FromResult(new iTalkDbContext().Users));
        }

        public ActionResult Login(string returnUrl = "/Admin/Home/Index") {
            if (this.User.Identity.IsAuthenticated) {
                return this.Redirect(returnUrl);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Title = "登入";

            return this.View();
        }
    }
}
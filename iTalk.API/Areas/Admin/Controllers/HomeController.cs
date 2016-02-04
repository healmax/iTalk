using iTalk.API.Models;
using iTalk.API.Properties;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTalk.API.Areas.Admin.Controllers {
    /// <summary>
    /// 主控制器
    /// </summary>
    [Authorize]
    public class HomeController : Controller {
        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns>首頁</returns>        
        public async Task<ActionResult> Index() {
            this.ViewBag.Title = Resources.HomePage;

            iTalkClient client = new iTalkClient();
            var response = await client.GetAsync("account?userName=" + this.User.Identity.Name);
            response.EnsureSuccessStatusCode();

            UserInfoBase model = (await response.Content.ReadAsAsync<ExecuteResult<UserInfoBase>>()).Result;

            return this.View(model);
        }
    }
}
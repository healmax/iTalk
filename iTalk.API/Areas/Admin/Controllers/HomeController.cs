using iTalk.API.Properties;
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
        public ActionResult Index() {
            this.ViewBag.Title = Resources.HomePage;
            return this.View();
        }
    }
}
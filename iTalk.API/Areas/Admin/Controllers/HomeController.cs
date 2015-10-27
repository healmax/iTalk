using iTalk.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
            return this.View();
        }
    }
}
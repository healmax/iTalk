using iTalk.API.Areas.Admin.Models;
using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iTalk.API.Areas.Admin.Controllers {
    /// <summary>
    /// 帳戶控制器
    /// </summary>
    public class AccountController : Controller {
        /// <summary>
        /// 列出所有使用者
        /// </summary>
        /// <returns>使用者列表頁面</returns>
        public async Task<ActionResult> Index() {
            return this.View(await Task.FromResult(new iTalkDbContext().Users));
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="returnUrl">Return URL</param>
        /// <returns>登入頁面</returns>
        public ActionResult Login(string returnUrl = "/Admin/Home/Index") {
            if (this.User.Identity.IsAuthenticated) {
                return this.Redirect(returnUrl);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Title = Resources.Login;

            return this.View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model">帳戶 View Model</param>
        /// <param name="returnUrl">Return URL</param>
        /// <returns>登入結果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountViewModel model, string returnUrl = "/Admin/Home/Index") {
            if (this.ModelState.IsValid) {
                string data = string.Format("grant_type=password&username={0}&password={1}", model.UserName, model.Password);
                var response = await new iTalkClient().PostAsync("/Token", new StringContent(data));

                if (response.StatusCode == HttpStatusCode.OK) {
                    string authCookie = response.Headers.GetValues("Set-Cookie").First();
                    string[] parts = authCookie.Split(';')[0].Split('=');
                    this.Response.AppendCookie(new HttpCookie(parts[0], parts[1]));
                    return this.RedirectToAction("Index", "Home");
                }

                var json = await response.Content.ReadAsStringAsync();
                try {
                    dynamic result = JValue.Parse(json);
                    this.ModelState.AddModelError(string.Empty, (string)result.error_description);
                }
                catch (Exception) {
                    this.ModelState.AddModelError(string.Empty, json);
                }
            }

            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Title = Resources.Login;

            return this.View(model);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>登入頁面</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout() {
            this.HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            return this.Redirect("Login");
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <returns>註冊頁面</returns>
        public ViewResult Register(string returnUrl = "/Account/Home/Index") {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Title = Resources.iTalk + Resources.Account;

            return this.View();
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model">註冊資料</param>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>註冊結果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(FormCollection form, string returnUrl = "/Account/Home/Index") {
            RegisterViewModel model = new RegisterViewModel();
            model.Alias = form["Alias"];
            model.Password = form["Password"];
            model.PersonalSign = form["PersonalSign"];
            model.UserName = form["UserName"];
            this.UpdateModel<RegisterViewModel>(model);

            if (this.ModelState.IsValid) {
                HttpResponseMessage response;
                HttpPostedFileBase portrait = this.Request.Files["portrait"];

                MultipartFormDataContent content = new MultipartFormDataContent();

                if (!string.IsNullOrEmpty(model.Alias)) {
                    content.Add(new StringContent(model.Alias), "Alias");
                }

                content.Add(new StringContent(model.Password), "Password");

                if (!string.IsNullOrEmpty(model.PersonalSign)) {
                    content.Add(new StringContent(model.PersonalSign), "PersonalSign");
                }

                content.Add(new StringContent(model.UserName), "UserName");

                if (portrait != null && portrait.ContentLength != 0) {
                    content.Add(new StreamContent(portrait.InputStream), "portrait", portrait.FileName);
                }

                response = await new iTalkClient().PostAsync("Account", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsAsync<ExecuteResult>();

                if (result.Success) {
                    return await this.Login(model, returnUrl);
                }

                this.ModelState.AddModelError(string.Empty, result.Message);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Title = Resources.iTalk + Resources.Account;

            return this.View(model);
        }
    }
}
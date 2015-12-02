using iTalk.API.Models;
using iTalk.API.Properties;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Areas.Admin.Models {
    /// <summary>
    /// 註冊 View Model
    /// </summary>
    public class RegisterViewModel : AccountViewModel {
        /// <summary>
        /// 取得/設定 密碼確認
        /// </summary>
        [Required]
        [Compare("Password")]
        [Display(ResourceType = typeof(Resources), Name = "PasswordConfirm")]
        public string PasswordConfirm { get; set; }
    }
}
using iTalk.API.Models;
using iTalk.API.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iTalk.API.Areas.Admin.Models {
    /// <summary>
    /// 註冊 View Model
    /// </summary>
    public class RegisterViewModel : AccountViewModel {
        /// <summary>
        /// 密碼確認
        /// </summary>
        [Required]
        [Compare("Password")]
        [Display(ResourceType = typeof(Resources), Name = "PasswordConfirm")]
        public string PasswordConfirm { get; set; }
    }
}
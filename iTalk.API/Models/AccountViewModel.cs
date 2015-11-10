﻿using iTalk.API.Properties;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 帳戶用 View Model
    /// </summary>
    public class AccountViewModel {
        /// <summary>
        /// 取得/設定 使用者名稱
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        [RegularExpression("[A-Za-z0-@_]{3,10}", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserNameOrPasswordFormatError")]
        public string UserName { get; set; }

        /// <summary>
        /// 取得/設定 密碼
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        [RegularExpression("[A-Za-z0-@_]{3,10}", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserNameOrPasswordFormatError")]
        public string Password { get; set; }
    }
}
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
        [StringLength(10, MinimumLength = 3)]
        [RegularExpression("[A-Za-z0-@_]{3,10}")]
        public string UserName { get; set; }

        /// <summary>
        /// 取得/設定 密碼
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 3)]
        [RegularExpression("[A-Za-z0-@_]{3,10}")]
        public string Password { get; set; }
    }
}
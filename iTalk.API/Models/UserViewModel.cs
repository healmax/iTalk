using iTalk.API.Properties;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 使用者的基本資料
    /// </summary>
    public class UserViewModel {
        /// <summary>
        /// 取得/設定 使用者名稱
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        [RegularExpression("[A-Za-z0-@_]{3,20}", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserNameOrPasswordFormatError")]
        public string UserName { get; set; }

        /// <summary>
        /// 取得/設定 暱稱
        /// </summary>
        [StringLength(10, MinimumLength = 2)]
        [Display(ResourceType = typeof(Resources), Name = "Alias")]
        public virtual string Alias { get; set; }

        /// <summary>
        /// 取得/設定 個性簽名
        /// </summary>
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "PersonalSign")]
        public string PersonalSign { get; set; }
    }
}
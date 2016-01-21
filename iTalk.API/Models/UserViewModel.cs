using iTalk.API.Properties;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace iTalk.API.Models {
    /// <summary>
    /// 使用者的基本資料
    /// </summary>
    public class UserViewModelBase {
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
        [StringLength(10, MinimumLength = 3)]
        [Display(ResourceType = typeof(Resources), Name = "Alias")]
        public string Alias { get; set; }

        /// <summary>
        /// 取得/設定 個性簽名
        /// </summary>
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "PersonalSign")]
        public string PersonalSign { get; set; }
    }

    /// <summary>
    /// 使用者 View Model
    /// </summary>
    public class UserViewModel : UserViewModelBase {
        /// <summary>
        /// 使用者 Id
        /// </summary>
        [Required]
        public long Id { get; set; }

        ///// <summary>
        ///// 取得/設定 個人圖片
        ///// </summary>
        //public Stream Portrait { get; set; }
    }

    /// <summary>
    /// 回傳的使用者資訊
    /// </summary>
    public class UserInfo : UserViewModel {
        /// <summary>
        /// 取得/設定 個人圖片縮圖
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// 取得/設定 個人圖片網址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
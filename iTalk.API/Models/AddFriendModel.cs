using System.ComponentModel.DataAnnotations;
namespace iTalk.API.Models {
    /// <summary>
    /// 加好友 View Model
    /// </summary>
    public class AddFriendViewModel {
        /// <summary>
        /// 取得/設定 目標使用者 Id
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// 取得/設定 邀請訊息
        /// </summary>
        public string Content { get; set; }
    }
}
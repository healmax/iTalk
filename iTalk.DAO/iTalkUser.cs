using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iTalk.DAO {
    /// <summary>
    /// 使用者
    /// </summary>
    public class iTalkUser : IdentityUser<long, iTalkUserLogin, iTalkUserRole, iTalkUserClaim>, ITarget {
        /// <summary>
        /// 重寫 UserName
        /// </summary>
        [MinLength(3)]
        [MaxLength(20)]
        public override string UserName {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        /// <summary>
        /// 取得/設定 暱稱
        /// </summary>
        [MinLength(3)]
        [MaxLength(10)]
        public string Alias { get; set; }

        /// <summary>
        /// 取得/設定 個人簽名
        /// </summary>
        [MaxLength(50)]
        public string PersonalSign { get; set; }

        /// <summary>
        /// 取得/設定 個人圖片
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 取得/設定 個人圖片縮圖
        /// </summary>
        public byte[] Thumb { get; set; }

        ///// <summary>
        ///// 取得 發送的對話集合
        ///// </summary>
        //public virtual ICollection<Chat> SendedChats { get; private set; }

        ///// <summary>
        ///// 取得 接收的對話集合
        ///// </summary>
        //public virtual ICollection<Chat> ReceivedChats { get; private set; }

        ///// <summary>
        ///// 取得 主動發出邀請的關係集合
        ///// </summary>
        //public virtual ICollection<Friendship> ActiveShips { get; private set; }

        ///// <summary>
        ///// 取得 被動接受邀請的關係集合
        ///// </summary>
        //public virtual ICollection<Friendship> PassiveShips { get; private set; }

        ///// <summary>
        ///// 取得 建立的群組
        ///// </summary>
        //public virtual ICollection<Group> CreateGroups { get; private set; }

        ///// <summary>
        ///// 取得 加入的群組
        ///// </summary>
        //public virtual ICollection<GroupMember> GroupMembers { get; private set; }
    }
}

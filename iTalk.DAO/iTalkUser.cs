using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iTalk.DAO {
    /// <summary>
    /// 使用者
    /// </summary>
    public class iTalkUser : IdentityUser {
        /// <summary>
        /// 重寫 UserName
        /// </summary>
        [StringLength(10, MinimumLength = 3)]
        public override string UserName {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        /// <summary>
        /// 取得 發送的對話集合
        /// </summary>
        public virtual ICollection<Chat> SendedChats { get; private set; }

        /// <summary>
        /// 取得 接收的對話集合
        /// </summary>
        public virtual ICollection<Chat> ReceivedChats { get; private set; }

        /// <summary>
        /// 取得 主動發出邀請的關係集合
        /// </summary>
        public virtual ICollection<Relationship> ActiveShips { get; private set; }

        /// <summary>
        /// 取得 被動接受邀請的關係集合
        /// </summary>
        public virtual ICollection<Relationship> PassiveShips { get; private set; }
    }
}

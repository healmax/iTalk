using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 朋友關係
    /// </summary>
    public class Friendship : Relationship {
        /// <summary>
        /// 建構函數 For EF
        /// </summary>
        protected Friendship() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userId">邀請人 Id</param>
        /// <param name="InviteeId">受邀者 Id</param>
        /// <param name="userStatus">邀請人狀態</param>
        /// <param name="userReadTime">使用者最後讀取時間</param>
        /// <param name="inviteeStatus">受邀者狀態</param>
        /// <param name="inviteeReadTime">受邀者最後讀取時間</param>
        /// <param name="date">建立時間</param>
        public Friendship(long userId, long InviteeId, RelationshipStatus userStatus, DateTime userReadTime, RelationshipStatus inviteeStatus, DateTime inviteeReadTime, DateTime date)
            : base(date) {
            this.UserId = userId;
            this.UserStatus = userStatus;
            this.UserReadTime = userReadTime;
            this.InviteeId = InviteeId;
            this.InviteeStatus = inviteeStatus;
            this.InviteeReadTime = inviteeReadTime;
        }

        /// <summary>
        /// 取得 邀請者Id
        /// </summary>
        [Required]
        [Index("Friendship_Index", IsUnique = true, Order = 0)]
        public long UserId { get; private set; }

        /// <summary>
        /// 取得 使用者
        /// </summary>
        [ForeignKey("UserId")]
        public virtual iTalkUser User { get; private set; }

        /// <summary>
        /// 取得/設定 關係狀態
        /// </summary>
        [Required]
        public RelationshipStatus UserStatus { get; set; }

        /// <summary>
        /// 取得/設定 使用者上次讀取時間
        /// </summary>
        [Required]
        public DateTime UserReadTime { get; set; }

        /// <summary>
        /// 取得 受邀者 Id
        /// </summary>
        [Required]
        [Index("Friendship_Index", IsUnique = true, Order = 1)]
        public long InviteeId { get; private set; }

        /// <summary>
        /// 取得 受邀者
        /// </summary>
        [ForeignKey("InviteeId")]
        public virtual iTalkUser Invitee { get; private set; }

        /// <summary>
        /// 取得/設定 關係狀態
        /// </summary>
        [Required]
        public RelationshipStatus InviteeStatus { get; set; }

        /// <summary>
        /// 取得/設定 受邀者上次讀取時間
        /// </summary>
        [Required]
        public DateTime InviteeReadTime { get; set; }

        /// <summary>
        /// 取得/設定 讀取時間
        /// </summary>
        [NotMapped]
        public DateTime this[long id] {
            get { return this.UserId == id ? this.UserReadTime : this.InviteeReadTime; }
            set {
                if (this.UserId == id) {
                    this.UserReadTime = value;
                }
                else {
                    this.InviteeReadTime = value;
                }
            }
        }
    }
}

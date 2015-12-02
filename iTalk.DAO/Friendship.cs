using System;
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
        /// <param name="status">關係狀態</param>
        /// <param name="date">建立時間</param>
        public Friendship(long userId, long InviteeId, RelationshipStatus status, DateTime date)
            : base(date) {
            this.UserId = userId;
            this.InviteeId = InviteeId;
            this.Status = status;
        }

        /// <summary>
        /// 取得 邀請者Id
        /// </summary>
        [Required]
        [Index("Relations", IsUnique = true, Order = 0)]
        public long UserId { get; private set; }

        /// <summary>
        /// 取得 使用者
        /// </summary>
        [ForeignKey("UserId")]
        [InverseProperty("ActiveShips")]
        public virtual iTalkUser User { get; private set; }

        /// <summary>
        /// 取得 受邀者 Id
        /// </summary>
        [Required]
        [Index("Relations", IsUnique = true, Order = 1)]
        public long InviteeId { get; private set; }

        /// <summary>
        /// 取得 受邀者
        /// </summary>
        [ForeignKey("InviteeId")]
        [InverseProperty("PassiveShips")]
        public virtual iTalkUser Invitee { get; private set; }

        /// <summary>
        /// 取得/設定 關係狀態
        /// </summary>
        [Required]
        public RelationshipStatus Status { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 關係
    /// </summary>
    public class Relationship {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Relationship() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userId">邀請人 Id</param>
        /// <param name="InviteeId">受邀者 Id</param>
        public Relationship(string userId, string InviteeId) {
            this.UserId = userId;
            this.InviteeId = InviteeId;
        }

        /// <summary>
        /// 取得 主鍵
        /// </summary>

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        /// <summary>
        /// 取得 邀請者Id
        /// </summary>
        [Required]
        [Index("Relations", IsUnique = true, Order = 0)]
        public string UserId { get; private set; }

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
        public string InviteeId { get; private set; }

        /// <summary>
        /// 取得 受邀者
        /// </summary>
        [ForeignKey("InviteeId")]
        [InverseProperty("PassiveShips")]
        public virtual iTalkUser Invitee { get; private set; }
    }
}

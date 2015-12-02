﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 群組成員
    /// </summary>
    public class GroupMember : EntityBase {
        /// <summary>
        /// 建構函數 For EF
        /// </summary>
        protected GroupMember() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userId">成員 Id</param>
        /// <param name="groupId">邀請者 Id</param>
        /// <param name="status">關係狀態</param>
        /// <param name="date">建立時間</param>
        public GroupMember(long userId, long groupId, RelationshipStatus status, DateTime date)
            : base(date) {
            //if (string.IsNullOrEmpty(userId)) {
            //    throw new ArgumentNullException("memberId");
            //}

            //if (groupId == null) {
            //    throw new ArgumentNullException("groupId");
            //}

            this.UserId = userId;
            this.Status = status;
        }

        /// <summary>
        /// 取得 使用者Id
        /// </summary>
        [Required]
        [Index("Member", IsUnique = true, Order = 0)]
        public long UserId { get; private set; }

        /// <summary>
        /// 取得 使用者
        /// </summary>
        [ForeignKey("UserId")]
        [InverseProperty("GroupMembers")]
        public virtual iTalkUser User { get; private set; }

        /// <summary>
        /// 取得 群組 Id
        /// </summary>
        [Required]
        [Index("Member", IsUnique = true, Order = 1)]
        public long GroupId { get; private set; }

        /// <summary>
        /// 取得 群組
        /// </summary>
        [ForeignKey("GroupId")]
        [InverseProperty("Members")]
        public virtual Group Group { get; private set; }

        /// <summary>
        /// 取得/設定 關係狀態
        /// </summary>
        [Required]
        public RelationshipStatus Status { get; private set; }
    }
}

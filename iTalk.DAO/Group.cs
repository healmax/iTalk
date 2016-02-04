using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 群組
    /// </summary>
    public class Group : Relationship, IPortrait {
        /// <summary>
        /// 建構函數 For EF
        /// </summary>
        protected Group() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="name">群組名稱</param>
        /// <param name="creatorId">建立者 Id</param>
        /// <param name="date">建立時間</param>
        public Group(string name, long creatorId, DateTime date)
            : base(date) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            //if (string.IsNullOrEmpty(creatorId)) {
            //    throw new ArgumentNullException("creatorId");
            //}

            this.Name = name;
            this.CreatorId = creatorId;
        }

        /// <summary>
        /// 取得/設定 群組名稱
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 取得/設定 群組描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 取得 建立者 Id
        /// </summary>
        [Required]
        public long CreatorId { get; private set; }

        /// <summary>
        /// 取得 建立者
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual iTalkUser Creator { get; private set; }

        /// <summary>
        /// 取得 成員集合
        /// </summary>
        public virtual ICollection<GroupMember> Members { get; private set; }

        /// <summary>
        /// 取得 圖片 Id
        /// </summary>
        public long? PortraitId { get; set; }

        /// <summary>
        /// 取得 圖片
        /// </summary>
        [ForeignKey("PortraitId")]
        public virtual Portrait Portrait { get; set; }
    }
}

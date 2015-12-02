using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 群組
    /// </summary>
    public class Group : Relationship {
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

            this.CreatorId = creatorId;
            this.Name = name;
        }

        /// <summary>
        /// 取得/設定 群組名稱
        /// </summary>
        [Required]
        // TODO : enable this
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 取得/設定 群組描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 取得/設定 縮圖
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// 取得/設定 圖片網址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 取得 建立者 Id
        /// </summary>
        [Required]
        public long CreatorId { get; private set; }

        /// <summary>
        /// 取得 建立者
        /// </summary>
        [ForeignKey("CreatorId")]
        //[InverseProperty("CreateGroups")]
        public virtual iTalkUser Creator { get; private set; }

        /// <summary>
        /// 取得 成員集合
        /// </summary>
        public virtual ICollection<GroupMember> Members { get; private set; }
    }
}

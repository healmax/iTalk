using System;
using System.Collections.Generic;

namespace iTalk.API.Models {
    /// <summary>
    /// 群組結果
    /// </summary>
    public class GroupResult : GroupViewModelBase {
        /// <summary>
        /// 取得/設定 Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 取得/設定 縮圖
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// 取得/設定 圖片網址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 群組成員 Id 
        /// </summary>
        IEnumerable<GroupMember> _members;

        /// <summary>
        /// 取得/設定 群組成員 Id 
        /// </summary>
        public IEnumerable<GroupMember> Members {
            get {
                if (this._members == null) {
                    this._members = new List<GroupMember>();
                }

                return this._members;
            }
            set {
                this._members = value;
            }
        }

        /// <summary>
        /// 群組成員
        /// </summary>
        public class GroupMember {
            /// <summary>
            /// 取得/設定 成員 Id
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 取得/設定 最後讀取時間
            /// </summary>
            public DateTime ReadTime { get; set; }
        }
    }
}
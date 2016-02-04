using iTalk.DAO;
using System;
using System.Collections.Generic;

namespace iTalk.API.Models {
    /// <summary>
    /// 群組結果
    /// </summary>
    public class GroupInfo : GroupViewModelBase {
        /// <summary>
        /// 取得/設定 Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 縮圖
        /// </summary>
        string _thumbnail;

        /// <summary>
        /// 取得/設定 縮圖
        /// </summary>
        public string Thumbnail {
            get {
                if (string.IsNullOrEmpty(this._thumbnail)) {
                    this._thumbnail = this.PortraitUrl;
                }

                return this._thumbnail;
            }
            set {
                this._thumbnail = value;
            }
        }

        /// <summary>
        /// 取得/設定 圖片網址
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 取得/設定 未讀取訊息數量
        /// </summary>
        public int UnreadMessageCount { get; set; }

        /// <summary>
        /// 取得/設定 最後一則對話
        /// </summary>
        public Chat LastChat { get; set; }

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
            /// 取得/設定 主鍵
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 取得/設定 最後讀取時間
            /// </summary>
            public DateTime ReadTime { get; set; }
        }

        ///// <summary>
        ///// 群組成員 Id 
        ///// </summary>
        //IEnumerable<GroupMember> _members;

        ///// <summary>
        ///// 取得/設定 群組成員 Id 
        ///// </summary>
        //public IEnumerable<GroupMember> Members {
        //    get {
        //        if (this._members == null) {
        //            this._members = new List<GroupMember>();
        //        }

        //        return this._members;
        //    }
        //    set {
        //        this._members = value;
        //    }
        //}

        ///// <summary>
        ///// 群組成員
        ///// </summary>
        //public class GroupMember : UserInfoBase {
        //    /// <summary>
        //    /// 取得/設定 最後讀取時間
        //    /// </summary>
        //    public DateTime ReadTime { get; set; }
        //}
    }
}
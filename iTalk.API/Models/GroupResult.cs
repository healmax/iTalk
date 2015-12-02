using System.Collections.Generic;

namespace iTalk.API.Models {
    /// <summary>
    /// 群組結果
    /// </summary>
    public class GroupResult : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="groups">群組集合</param>
        public GroupResult(IEnumerable<GroupDetail> groups)
            : base() {
            this.Group = groups;
        }

        /// <summary>
        /// 取得 群組集合
        /// </summary>
        public IEnumerable<GroupDetail> Group { get; private set; }

        /// <summary>
        /// 回傳群組的詳細資料
        /// </summary>
        public class GroupDetail : GroupViewModel {
            /// <summary>
            /// 取得/設定 縮圖
            /// </summary>
            public byte[] Thumbnail { get; set; }

            /// <summary>
            /// 取得/設定 圖片網址
            /// </summary>
            public string ImageUrl { get; set; }
        }
    }
}
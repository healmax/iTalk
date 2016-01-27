namespace iTalk.API.Models {
    public class UserInfoBase : UserViewModel {
        /// <summary>
        /// 取得/設定  Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 取得/設定 個人圖片縮圖
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// 取得/設定 個人圖片網址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 取得/設定 暱稱
        /// </summary>
        public override string Alias {
            get { return base.Alias ?? this.UserName; }
            set { base.Alias = value; }
        }
    }
}
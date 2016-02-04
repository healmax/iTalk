namespace iTalk.API.Models {
    public class UserInfoBase : UserViewModel {
        /// <summary>
        /// 取得/設定  Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 個人圖片縮圖
        /// </summary>
        string _thumbnail;

        /// <summary>
        /// 取得/設定 個人圖片縮圖
        /// </summary>
        public string Thumbnail {
            get {
                if (string.IsNullOrEmpty(this._thumbnail)) {
                    this._thumbnail = this.PortraitUrl;
                }

                return this._thumbnail;
            }
            set { this._thumbnail = value; }
        }

        /// <summary>
        /// 取得/設定 個人圖片網址
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 取得/設定 暱稱
        /// </summary>
        public override string Alias {
            get { return base.Alias ?? this.UserName; }
            set { base.Alias = value; }
        }
    }
}
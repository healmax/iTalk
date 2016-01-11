namespace iTalk.API.Models {
    /// <summary>
    /// 使用者 View Model
    /// </summary>
    public class UserResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        public UserResult() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="id">使用者 Id</param>
        /// <param name="userName">使用者名稱</param>
        /// <param name="alias">暱稱</param>
        /// <param name="personalSign">個性簽名</param>
        /// <param name="isFriend">是否為朋友</param>
        public UserResult(long id, string userName, string alias, string personalSign, bool isFriend) {
            this.Id = id;
            this.UserName = userName;
            this.Alias = alias;
            this.PersonalSign = personalSign;
            this.IsFriend = isFriend;
        }

        /// <summary>
        /// 取得/設定 Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 取得/設定 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 取得 暱稱
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 個性簽名
        /// </summary>
        string _personalSign;

        /// <summary>
        /// 取得 個性簽名
        /// </summary>
        public string PersonalSign {
            get { return string.IsNullOrEmpty(this._personalSign) ? this.UserName : this._personalSign; }
            set { this._personalSign = value; }
        }

        /// <summary>
        /// 取得/設定 是否為朋友
        /// </summary>
        public bool IsFriend { get; set; }
    }
}
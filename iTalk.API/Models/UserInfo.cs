namespace iTalk.API.Models {
    /// <summary>
    /// 使用者 View Model
    /// </summary>
    public class UserInfo : UserInfoBase {
        /// <summary>
        /// 建構函數
        /// </summary>
        public UserInfo() { }

        ///// <summary>
        ///// 建構函數
        ///// </summary>
        ///// <param name="id">使用者 Id</param>
        ///// <param name="userName">使用者名稱</param>
        ///// <param name="alias">暱稱</param>
        ///// <param name="personalSign">個性簽名</param>
        ///// <param name="isFriend">是否為朋友</param>
        //public UserInfo(long id, string userName, string alias, string personalSign, bool isFriend) {
        //    this.Alias = alias;
        //    this.Id = id;
        //    this.IsFriend = isFriend;
        //    this.PersonalSign = personalSign;
        //    this.UserName = userName;
        //    this.ImageUrl = 
        //}

        /// <summary>
        /// 取得/設定 是否為朋友
        /// </summary>
        public virtual bool IsFriend { get; set; }
    }
}
namespace iTalk.API.Models {
    /// <summary>
    /// 關係的狀態
    /// </summary>
    public enum RelationshipStatus {
        /// <summary>
        /// 使用者不存在
        /// </summary>
        NotExist,

        /// <summary>
        /// 不是朋友
        /// </summary>
        NotFriend,

        /// <summary>
        /// 朋友
        /// </summary>
        Friend
    }
}
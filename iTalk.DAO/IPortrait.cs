namespace iTalk.DAO {
    /// <summary>
    /// 儲存圖片的實體
    /// </summary>
    public interface IPortrait {
        /// <summary>
        /// 取得 圖片 Id
        /// </summary>
        long? PortraitId { get; set; }

        /// <summary>
        /// 取得 圖片
        /// </summary>
        Portrait Portrait { get; set; }
    }
}

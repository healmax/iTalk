using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 圖片
    /// </summary>
    public class Portrait : EntityBase {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Portrait() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="filename">檔案名稱</param>
        /// <param name="date">檔案名稱</param>
        /// <param name="content">圖片</param>
        /// <param name="thumbnail">縮圖</param>
        public Portrait(string filename, byte[] content, string thumbnail, DateTime date)
            : base(date) {
            if (string.IsNullOrEmpty(filename)) {
                throw new ArgumentNullException("filename");
            }

            if (content == null) {
                throw new ArgumentNullException("portrait");
            }

            this.Filename = filename;
            this.Content = content;
            this.Thumbnail = thumbnail;
        }

        /// <summary>
        /// 取得/設定 圖片內容
        /// </summary>
        [Required]
        public byte[] Content { get; set; }

        /// <summary>
        /// 取得/設定 圖片名稱
        /// </summary>
        [Required]
        [MaxLength(36)]
        [Index(IsUnique = true)]
        public string Filename { get; set; }

        /// <summary>
        /// 取得/設定 縮圖
        /// </summary>
        public string Thumbnail { get; set; }
    }
}

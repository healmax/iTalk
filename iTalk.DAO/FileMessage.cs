using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 檔案訊息
    /// </summary>
    public class FileMessage : Chat {
        /// <summary>
        /// 建構函數 For EF
        /// </summary>
        protected FileMessage() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="senderId">發送者 Id</param>
        /// <param name="receiverId">接收者 Id</param>
        /// <param name="date">對話日期</param>
        /// <param name="filename">檔案名稱</param>
        /// <param name="url">Url</param>
        /// <param name="mimeType">Mime Type</param>
        /// <param name="size">檔案大小</param>
        /// <param name="thumbnail">縮圖</param>
        public FileMessage(long senderId, long receiverId, DateTime date, string filename, string url, string mimeType, long size, byte[] thumbnail)
            : base(receiverId, senderId, date) {
            if (string.IsNullOrEmpty(filename)) {
                throw new ArgumentNullException("filename");
            }

            if (string.IsNullOrEmpty(url)) {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrEmpty(mimeType)) {
                throw new ArgumentNullException("mimeType");
            }

            this.FileName = filename;
            this.Url = url;
            this.MimeType = mimeType;
            this.Size = size;
            this.Thumbnail = thumbnail;
        }

        /// <summary>
        /// 取得 檔案名稱
        /// </summary>
        [Required]
        public string FileName { get; private set; }

        /// <summary>
        /// 取得 Url
        /// </summary>
        [Required]
        public string Url { get; private set; }

        /// <summary>
        /// 取得 Mime Type
        /// </summary>
        [Required]
        public string MimeType { get; private set; }

        /// <summary>
        /// 取得 檔案大小
        /// </summary>
        [Required]
        public long Size { get; private set; }

        /// <summary>
        /// 取得 縮圖 (如果是圖片)
        /// </summary>
        public byte[] Thumbnail { get; private set; }
    }
}

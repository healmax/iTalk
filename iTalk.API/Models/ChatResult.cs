using iTalk.DAO;
using System;
using System.Collections.Generic;

namespace iTalk.API.Models {
    /// <summary>
    /// 對話結果
    /// </summary>
    public class ChatResult : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        public ChatResult(IEnumerable<Chat> chats) {
            if (chats == null) {
                throw new ArgumentNullException("chats");
            }

            this.Chats = chats;
        }

        ///// <summary>
        ///// 對話集合
        ///// </summary>
        //List<ChatDetail> _chats;

        /// <summary>
        /// 取得 對話集合
        /// </summary>
        public IEnumerable<Chat> Chats { get; private set; }

        ///// <summary>
        ///// 對話細節
        ///// </summary>
        //public abstract class ChatDetail {
        //    public ChatDetail() { }

        //    /// <summary>
        //    /// 建構函數
        //    /// </summary>
        //    /// <param name="chat">對話</param>
        //    public ChatDetail(Chat chat) {
        //        if (chat == null) {
        //            throw new ArgumentNullException("chat");
        //        }

        //        this.Id = chat.Id;
        //        this.Date = chat.Date;
        //        this.Sender = chat.Sender.UserName;

        //        if (chat.Relationship as Friendship != null) {
        //            this.Receiver = (chat.Relationship as Friendship).Invitee.UserName;
        //        }
        //        else if (chat.Relationship as Group != null) {
        //            this.Receiver = (chat.Relationship as Group).Id;
        //        }
        //    }

        //    public Guid Id { get; set; }

        //    /// <summary>
        //    /// 取得 發送對話者
        //    /// </summary>
        //    public string Sender { get; set; }

        //    /// <summary>
        //    /// 取得 對話接收人
        //    /// </summary>
        //    public dynamic Receiver { get; set; }

        //    /// <summary>
        //    /// 取得 對話時間
        //    /// </summary>
        //    public DateTime Date { get; set; }
        //}

        ///// <summary>
        ///// 對話
        ///// </summary>
        //public class DialogDetail : ChatDetail {
        //    public DialogDetail() { }

        //    /// <summary>
        //    /// 建構函數
        //    /// </summary>
        //    /// <param name="dialog">對話</param>
        //    public DialogDetail(Dialog dialog)
        //        : base(dialog) {
        //        this.Content = dialog.Content;
        //    }

        //    /// <summary>
        //    /// 取得 對話內容
        //    /// </summary>
        //    public string Content { get; set; }
        //}

        ///// <summary>
        ///// 檔案訊息
        ///// </summary>
        //public class FileMessageDetail : ChatDetail {
        //    public FileMessageDetail() { }

        //    /// <summary>
        //    /// 建構函數
        //    /// </summary>
        //    /// <param name="fileMessage">檔案訊息</param>
        //    public FileMessageDetail(FileMessage fileMessage)
        //        : base(fileMessage) {
        //        this.FileName = fileMessage.FileName;
        //        this.MimeType = fileMessage.MimeType;
        //        this.Size = fileMessage.Size;
        //        this.Thumbnail = fileMessage.Thumbnail;
        //        this.Url = fileMessage.Url;
        //    }

        //    /// <summary>
        //    /// 取得 檔案名稱
        //    /// </summary>
        //    public string FileName { get; set; }

        //    /// <summary>
        //    /// 取得 Url
        //    /// </summary>
        //    public string Url { get; set; }

        //    /// <summary>
        //    /// 取得 Mime Type
        //    /// </summary>
        //    public string MimeType { get; set; }

        //    /// <summary>
        //    /// 取得 檔案大小
        //    /// </summary>
        //    public long Size { get; set; }

        //    /// <summary>
        //    /// 取得 縮圖 (如果是圖片)
        //    /// </summary>
        //    public byte[] Thumbnail { get; set; }
        //}
    }
}
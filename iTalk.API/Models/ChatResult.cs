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
            if (chats != null) {
                foreach (Chat c in chats) {
                    this._chats.Add(new ChatDetail(c));
                }
            }
        }

        /// <summary>
        /// 對話集合
        /// </summary>
        List<ChatDetail> _chats = new List<ChatDetail>();

        /// <summary>
        /// 取得 對話集合
        /// </summary>
        public IEnumerable<ChatDetail> Chats {
            get { return this._chats; }
        }

        /// <summary>
        /// 對話細節
        /// </summary>
        public class ChatDetail {
            /// <summary>
            /// 建構函數
            /// </summary>
            /// <param name="chat">對話</param>
            public ChatDetail(Chat chat) {
                this.Content = chat.Content;
                this.Date = chat.Date;
                this.Sender = chat.Sender.UserName;
            }

            /// <summary>
            /// 取得 對話內容
            /// </summary>
            public string Content { get; private set; }

            /// <summary>
            /// 取得 對話時間
            /// </summary>
            public DateTime Date { get; private set; }

            /// <summary>
            /// 取得 發送對話者
            /// </summary>
            public string Sender { get; private set; }
        }
    }
}
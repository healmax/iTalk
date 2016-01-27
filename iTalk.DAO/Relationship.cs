using System;
using System.Collections.Generic;

namespace iTalk.DAO {
    /// <summary>
    /// 關係
    /// </summary>
    public abstract class Relationship : EntityBase {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Relationship() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        public Relationship(DateTime date)
            : base(date) {
        }

        /// <summary>
        /// 取得 此段關係相關的對話
        /// </summary>
        public virtual ICollection<Chat> Chats { get; private set; }
    }
}

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 實體抽象基類
    /// </summary>
    public abstract class EntityBase {
        /// <summary>
        /// 建構函數 For EF
        /// </summary>
        protected EntityBase() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="date">建立時間</param>
        protected EntityBase(DateTime date) {
            this.Date = date;
        }

        /// <summary>
        /// 取得 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        /// <summary>
        /// 取得 建立時間
        /// </summary>
        [Required]
        public DateTime Date { get; private set; }

        /// <summary>
        /// 取得 Time Stamp
        /// </summary>
        [Timestamp]
        [JsonIgnore]
        public byte[] TimeStamp { get; private set; }
    }
}

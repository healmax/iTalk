using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTalk.DAO {
    /// <summary>
    /// 關係狀態
    /// </summary>
    public enum RelationshipStatus {
        /// <summary>
        /// 未決定
        /// </summary>
        Pending,

        /// <summary>
        /// 接受
        /// </summary>
        Accepted,

        /// <summary>
        /// 拒絕
        /// </summary>
        Declined,

        /// <summary>
        /// 封鎖
        /// </summary>
        Blocked
    }
}

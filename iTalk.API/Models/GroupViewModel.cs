using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 建立群組 View Model
    /// </summary>
    public abstract class GroupViewModelBase {
        /// <summary>
        /// 建構函數
        /// </summary>
        public GroupViewModelBase() { }

        /// <summary>
        /// 取得/設定 名稱
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 取得/設定 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 群組 View Model
    /// </summary>
    public class GroupViewModel : GroupViewModelBase {
        /// <summary>
        /// 群組成員 Id 
        /// </summary>
        IEnumerable<long> _members;

        /// <summary>
        /// 取得/設定 群組成員 Id 
        /// </summary>
        public IEnumerable<long> Members {
            get {
                if (this._members == null) {
                    this._members = new List<long>();
                }

                return this._members;
            }
            set {
                this._members = value;
            }
        }
    }
}
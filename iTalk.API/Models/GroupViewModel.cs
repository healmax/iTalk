using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 建立群組 View Model
    /// </summary>
    public abstract class CreateGroupViewModel {
        /// <summary>
        /// 建構函數
        /// </summary>
        public CreateGroupViewModel() {
            this.Members = new List<long>();
        }

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

        /// <summary>
        /// 取得/設定 群組成員 Id 
        /// </summary>
        public List<long> Members { get; private set; }
    }

    /// <summary>
    /// 群組 View Model
    /// </summary>
    public class GroupViewModel : CreateGroupViewModel {
        /// <summary>
        /// 取得/設定 群組 Id
        /// </summary>
        public long Id { get; set; }
    }
}
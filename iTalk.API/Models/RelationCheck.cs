namespace iTalk.API.Models {
    //public class RelationValidation<TR>
    //        where TR : Relationship {
    //    public RelationValidation(TR relation, RelationCheck check) {
    //        this.Relation = relation;
    //        this.RelationCheck = check;
    //    }
    //    public TR Relation { get; private set; }

    //    public RelationCheck RelationCheck { get; private set; }
    //}

    //public class RelationValidation<TR, T> : RelationValidation<TR>
    //    where TR : Relationship
    //    where T : class {
    //    public RelationValidation(TR relation, RelationCheck check, T data = null)
    //        : base(relation, check) {
    //        this.Data = data;
    //    }

    //    public T Data { get; private set; }
    //}

    /// <summary>
    /// 關係的狀態
    /// </summary>
    public enum RelationCheck {
        /// <summary>
        /// 關係不存在
        /// </summary>
        NotExist,

        /// <summary>
        /// 不是朋友或不在群組中
        /// </summary>
        NoRelation,

        /// <summary>
        /// 朋友或群組中
        /// </summary>
        HasRelation
    }
}
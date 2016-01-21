//using iTalk.DAO;
//using System.Collections.Generic;
//namespace iTalk.API.Models {
//    /// <summary>
//    /// 取得朋友的結果
//    /// </summary>
//    public class FriendResult {
//        /// <summary>
//        /// 建構函數
//        /// </summary>
//        /// <param name="friends">朋友名稱集合</param>
//        public FriendResult(IEnumerable<UserInfo> friends) {
//            this.Friends = friends;

//            //foreach (var f in friends) {
//            //    this.Friends.Add(new UserInfo() {
//            //        Alias =f .Alias,
//            //        PersonalSign = f.PersonalSign,
//            //        Thumb = f.Thumb,
//            //        UserName = f.UserName
//            //    });
//            //}
//        }

//        /// <summary>
//        /// 取得 朋友集合
//        /// </summary>
//        public IEnumerable<UserInfo> Friends { get; private set; }
//    }
//}
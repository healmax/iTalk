using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Diagnostics;

namespace iTalk.DAO {
    /// <summary>
    /// iTalk DB Context
    /// </summary>
    public class iTalkDbContext : IdentityDbContext<iTalkUser, iTalkRole, long, iTalkUserLogin, iTalkUserRole, iTalkUserClaim> {
        /// <summary>
        /// 建構函數
        /// </summary>
        public iTalkDbContext()
            : base("iTalk") {
#if DEBUG
            this.Database.Log = log => Debug.Write(log);
#endif
        }

        /// <summary>
        /// 取得 所有對話
        /// </summary>
        public virtual DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// 取得 文字對話
        /// </summary>
        public virtual DbSet<Dialog> Dialog { get; set; }

        /// <summary>
        /// 取得 傳送檔案對話
        /// </summary>
        public virtual DbSet<FileMessage> Files { get; set; }

        ///// <summary>
        ///// 取得 關係
        ///// </summary>
        //public virtual DbSet<Relationship> Relationships { get; set; }

        /// <summary>
        /// 取得 朋友關係
        /// </summary>
        public virtual DbSet<Friendship> Friendships { get; set; }

        /// <summary>
        /// 取得 群組
        /// </summary>
        public virtual DbSet<Group> Groups { get; set; }

        /// <summary>
        /// 取得 群組成員
        /// </summary>
        public virtual DbSet<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// 自訂 table relationship
        /// </summary>
        /// <param name="modelBuilder">DbModelBuilder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Friendship>()
                .Map(m => {
                    m.MapInheritedProperties();
                    m.ToTable("Friendships");
                });

            modelBuilder.Entity<Group>()
                .Map(m => {
                    m.MapInheritedProperties();
                    m.ToTable("Group");
                });

            modelBuilder.Entity<Group>()
                .HasRequired(g => g.Creator)
                .WithMany()
                .HasForeignKey(g => g.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .HasRequired(f => f.Invitee)
                .WithMany()
                .HasForeignKey(f => f.InviteeId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Friendship>()
            //    .HasRequired(f => f.User)
            //    .WithMany()
            //    .HasForeignKey(f => f.UserId)
            //    .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 創建實例
        /// </summary>
        /// <returns>iTalkDbContext</returns>
        public static iTalkDbContext Create() {
            return new iTalkDbContext();
        }
    }
}

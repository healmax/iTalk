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
            // Debug migration code
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<iTalkDbContext, Configuration>());
#endif
        }

        /// <summary>
        /// 取得 所有對話
        /// </summary>
        public virtual DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// 取得 文字對話
        /// </summary>
        public virtual DbSet<Dialog> Dialogs { get; set; }

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
        /// 取得 圖片
        /// </summary>
        public virtual DbSet<Portrait> Portraits { get; set; }

        /// <summary>
        /// 自訂 table relationship
        /// </summary>
        /// <param name="modelBuilder">DbModelBuilder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Group>()
                .Map(m => {
                    m.MapInheritedProperties();
                    m.ToTable("Groups");
                })
                .HasRequired(g => g.Creator)
                .WithMany(u => u.CreateGroups)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .Map(m => {
                    m.MapInheritedProperties();
                    m.ToTable("Friendships");
                });

            modelBuilder.Entity<Friendship>()
                .HasRequired(f => f.User)
                .WithMany(u => u.ActiveShips)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .HasRequired(f => f.Invitee)
                .WithMany(f => f.PassiveShips)
                .WillCascadeOnDelete(false);

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

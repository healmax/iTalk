using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Diagnostics;

namespace iTalk.DAO {
    /// <summary>
    /// iTalk DB Context
    /// </summary>
    public class iTalkDbContext : IdentityDbContext<iTalkUser> {
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
        /// 取得 對話
        /// </summary>
        public virtual DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// 取得 朋友
        /// </summary>
        public virtual DbSet<Relationship> Relationships { get; set; }

        /// <summary>
        /// 自訂 table relationship
        /// </summary>
        /// <param name="modelBuilder">DbModelBuilder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Chat>()
                .HasRequired(c => c.Sender)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chat>()
                .HasRequired(c => c.Receiver)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Relationship>()
                .HasRequired(rs => rs.User)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Relationship>()
                .HasRequired(rs => rs.Invitee)
                .WithMany()
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

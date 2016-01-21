using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;

namespace iTalk.DAO.Migrations {
    /// <summary>
    /// Entity framework code first migration configuration
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<iTalkDbContext> {
        /// <summary>
        /// 建構函數
        /// </summary>
        public Configuration() {
            this.AutomaticMigrationsEnabled = false;
            this.SetSqlGenerator("System.Data.SqlClient", new iTalkMigrationSqlGenerator());
        }

        /// <summary>
        /// 自動新增預設使用者
        /// </summary>
        /// <param name="context">iTalkDbContext</param>
        protected override void Seed(iTalkDbContext context) {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            PasswordHasher hasher = new PasswordHasher();
            context.Users.AddOrUpdate(u => u.Id, new[] {
                new iTalkUser() {
                    UserName = "TestUser1",
                    Alias = "翻譯機器人一號",
                    PersonalSign = "中翻英",
                    PasswordHash = hasher.HashPassword("111"),
                    SecurityStamp = Guid.NewGuid().ToString("D") },
                new iTalkUser() {
                    UserName = "TestUser2",
                    Alias = "測試使用者",
                    PersonalSign = "I'm Auto Create Test User",
                    PasswordHash = hasher.HashPassword("222"),
                    SecurityStamp = Guid.NewGuid().ToString("D") }
            });
        }
    }
}

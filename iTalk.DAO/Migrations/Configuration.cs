using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;

namespace iTalk.DAO.Migrations {
    /// <summary>
    /// Entity framework code first migration configuration
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<iTalkDbContext> {
        /// <summary>
        /// �غc���
        /// </summary>
        public Configuration() {
            this.AutomaticMigrationsEnabled = false;
            this.SetSqlGenerator("System.Data.SqlClient", new iTalkMigrationSqlGenerator());
        }

        /// <summary>
        /// �۰ʷs�W�w�]�ϥΪ�
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
                    Alias = "½Ķ�����H�@��",
                    PersonalSign = "��½�^",
                    PasswordHash = hasher.HashPassword("111"),
                    SecurityStamp = Guid.NewGuid().ToString("D") },
                new iTalkUser() {
                    UserName = "TestUser2",
                    Alias = "���ըϥΪ�",
                    PersonalSign = "I'm Auto Create Test User",
                    PasswordHash = hasher.HashPassword("222"),
                    SecurityStamp = Guid.NewGuid().ToString("D") }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.SqlServer;

namespace iTalk.DAO.Migrations {
    /// <summary>
    /// 自訂 Migration 時產生 Sql 的邏輯
    /// </summary>
    class iTalkMigrationSqlGenerator : SqlServerMigrationSqlGenerator {
        /// <summary>
        /// 強行改寫建立 Groups Table 的 Sql，將Identity 改為 -1 開始並遞減 1 
        /// </summary>
        /// <param name="migrationOperations">migrationOperations</param>
        /// <param name="providerManifestToken">providerManifestToken</param>
        /// <returns></returns>
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken) {
            var statements = base.Generate(migrationOperations, providerManifestToken);

            foreach (MigrationStatement s in statements) {
                if (s.Sql.StartsWith("create table [dbo].[groups]", StringComparison.OrdinalIgnoreCase)) {
                    string identity = "IDENTITY";
                    int index = s.Sql.IndexOf(identity, StringComparison.OrdinalIgnoreCase);
                    s.Sql = s.Sql.Insert(index + identity.Length, "(-1,-1)");
                }
                //else if (s.Sql.StartsWith("CREATE UNIQUE INDEX [IX_Filename]", StringComparison.OrdinalIgnoreCase)) {
                //    s.Sql += " WHERE [Portrait] IS NOT NULL";
                //}
            }

            return statements;
        }

        protected override void Generate(DropIndexOperation dropIndexOperation) {
            base.Generate(dropIndexOperation);
        }

        ///// <summary>
        ///// 建立群組 Table 時，將主鍵的自動增量設為 -1
        ///// </summary>
        ///// <param name="createTableOperation">CreateTableOperation</param>
        //protected override void WriteCreateTable(CreateTableOperation createTableOperation) {
        //    // TODO : 改良寫法
        //    if (createTableOperation.Name == "dbo.Groups") {
        //        foreach (var item in createTableOperation.PrimaryKey.AnonymousArguments) {
        //            System.Diagnostics.Debug.Write("key:" + item.Key + " value:" + item.Value);
        //        }

        //        base.Generate(new SqlOperation(
        //            "CREATE TABLE [dbo].[Groups](" +
        //                "[Id] [bigint] IDENTITY(-1,-1) NOT NULL," +
        //                "[Date] [datetime] NOT NULL," +
        //                "[TimeStamp] [timestamp] NOT NULL," +
        //                "[Name] [nvarchar](20) NOT NULL," +
        //                "[Description] [nvarchar](max) NULL," +
        //                "[Thumbnail] [nvarchar](max) NULL," +
        //                "[Portrait] [varbinary](max) NULL," +
        //                "[PortraitUrl] [nvarchar](32) NULL," +
        //                "[CreatorId] [bigint] NOT NULL," +
        //             "CONSTRAINT [PK_dbo.Group] PRIMARY KEY CLUSTERED " +
        //            "(" +
        //                "[Id] ASC" +
        //            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
        //            ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] "));
        //    }
        //    else {
        //        base.WriteCreateTable(createTableOperation);
        //    }
        //}

        //protected override void Generate(AddPrimaryKeyOperation addPrimaryKeyOperation) {
        //    if (addPrimaryKeyOperation.Table == "Group") {
        //        base.Generate("ALTER TABLE dbo.Group ADD Id BIGINT IDENTITY(-1,-1)");
        //    }
        //    else {
        //        base.Generate(addPrimaryKeyOperation);
        //    }
        //}
    }
}

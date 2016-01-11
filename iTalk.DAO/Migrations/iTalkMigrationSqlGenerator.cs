using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace iTalk.DAO.Migrations {
    /// <summary>
    /// 自訂 Migration 時產生 Sql 的邏輯
    /// </summary>
    class iTalkMigrationSqlGenerator : SqlServerMigrationSqlGenerator {
        /// <summary>
        /// 建立群組 Table 時，將主鍵的自動增量設為 -1
        /// </summary>
        /// <param name="createTableOperation">CreateTableOperation</param>
        protected override void WriteCreateTable(CreateTableOperation createTableOperation) {
            // TODO : 改良寫法
            if (createTableOperation.Name == "dbo.Group") {
                base.Generate(new SqlOperation(
                    "CREATE TABLE [dbo].[Group](" +
                        "[Id] [bigint] IDENTITY(-1,-1) NOT NULL," +
                        "[Date] [datetime] NOT NULL," +
                        "[TimeStamp] [timestamp] NOT NULL," +
                        "[Name] [nvarchar](20) NOT NULL," +
                        "[Description] [nvarchar](max) NULL," +
                        "[Thumbnail] [varbinary](max) NULL," +
                        "[ImageUrl] [nvarchar](max) NULL," +
                        "[CreatorId] [bigint] NOT NULL," +
                     "CONSTRAINT [PK_dbo.Group] PRIMARY KEY CLUSTERED " +
                    "(" +
                        "[Id] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] "));
            }
            else {
                base.WriteCreateTable(createTableOperation);
            }
        }

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

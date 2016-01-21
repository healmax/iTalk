namespace iTalk.DAO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SenderId = c.Long(nullable: false),
                        ReceiverId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Content = c.String(maxLength: 255),
                        FileName = c.String(),
                        Url = c.String(),
                        MimeType = c.String(),
                        Size = c.Long(),
                        Thumbnail = c.Binary(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(maxLength: 10),
                        PersonalSign = c.String(maxLength: 50),
                        PortraitUrl = c.String(),
                        Thumb = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        InviteeId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        ReadTime = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InviteeId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => new { t.UserId, t.InviteeId }, unique: true, name: "Relations");
            
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        ReadTime = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.GroupId }, unique: true, name: "Member");
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(),
                        Thumbnail = c.Binary(),
                        ImageUrl = c.String(),
                        CreatorId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GroupMembers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "InviteeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chats", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Groups", new[] { "CreatorId" });
            DropIndex("dbo.GroupMembers", "Member");
            DropIndex("dbo.Friendships", "Relations");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Chats", new[] { "SenderId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
            DropTable("dbo.Friendships");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Chats");
        }
    }
}

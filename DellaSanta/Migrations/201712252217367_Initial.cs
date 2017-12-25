namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(nullable: false, maxLength: 256),
                        ClaimValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ClaimType })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.UserId, t.ClaimType }, unique: true, name: "AK_UserClaim");
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 20),
                        FirstName = c.String(maxLength: 20),
                        LastName = c.String(maxLength: 20),
                        Password = c.String(nullable: false),
                        Role = c.String(nullable: false, maxLength: 10),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "AK_User_UserName");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.Users", "AK_User_UserName");
            DropIndex("dbo.UserClaims", "AK_UserClaim");
            DropTable("dbo.Users");
            DropTable("dbo.UserClaims");
        }
    }
}

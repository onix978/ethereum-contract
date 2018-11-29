namespace SmartContractEthereum.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EthereumAddress = c.String(maxLength: 8000, unicode: false),
                        Login = c.String(nullable: false, maxLength: 25, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        Created = c.DateTime(),
                        Updated = c.DateTime(),
                        Active = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EthereumContract",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContractID = c.String(maxLength: 150, unicode: false),
                        Created = c.DateTime(),
                        Updated = c.DateTime(),
                        Active = c.Boolean(),
                        Account_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Account", t => t.Account_ID)
                .Index(t => t.Account_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EthereumContract", "Account_ID", "dbo.Account");
            DropIndex("dbo.EthereumContract", new[] { "Account_ID" });
            DropTable("dbo.EthereumContract");
            DropTable("dbo.Account");
        }
    }
}

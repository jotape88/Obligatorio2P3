namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablaauxidsinautogenerar : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Auxiliar");
            AlterColumn("dbo.Auxiliar", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Auxiliar", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Auxiliar");
            AlterColumn("dbo.Auxiliar", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Auxiliar", "Id");
        }
    }
}

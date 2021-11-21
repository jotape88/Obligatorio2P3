namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregadoidentityenclaseparametros : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ParametrosAuxiliares");
            AlterColumn("dbo.ParametrosAuxiliares", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ParametrosAuxiliares", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ParametrosAuxiliares");
            AlterColumn("dbo.ParametrosAuxiliares", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ParametrosAuxiliares", "Id");
        }
    }
}

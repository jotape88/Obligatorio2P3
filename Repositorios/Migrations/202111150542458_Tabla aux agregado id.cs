namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablaauxagregadoid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Auxiliar");
            AddColumn("dbo.Auxiliar", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Auxiliar", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Auxiliar");
            DropColumn("dbo.Auxiliar", "Id");
            AddPrimaryKey("dbo.Auxiliar", new[] { "DescuentoPorAntiguedad", "TopeAntiguedad", "DescPorTopeActiv", "TopeActividades", "ValorMes", "ValorActividad" });
        }
    }
}

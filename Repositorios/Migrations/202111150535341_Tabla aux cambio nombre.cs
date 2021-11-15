namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablaauxcambionombre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntidadAuxiliars", newName: "Auxiliar");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Auxiliar", newName: "EntidadAuxiliars");
        }
    }
}

namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tabla_param_cambio_nombre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Auxiliar", newName: "ParametrosAuxiliares");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ParametrosAuxiliares", newName: "Auxiliar");
        }
    }
}

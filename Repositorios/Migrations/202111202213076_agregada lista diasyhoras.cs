namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregadalistadiasyhoras : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Actividades", new[] { "Nombre" });
            AlterColumn("dbo.Actividades", "Nombre", c => c.String(nullable: false, maxLength: 25));
            CreateIndex("dbo.Actividades", "Nombre", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Actividades", new[] { "Nombre" });
            AlterColumn("dbo.Actividades", "Nombre", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Actividades", "Nombre", unique: true);
        }
    }
}

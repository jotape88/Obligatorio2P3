namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removidonvarcharmaxdeestaactivo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Usuarios", new[] { "Email" });
            AlterColumn("dbo.Socios", "EstaActivo", c => c.String(maxLength: 1));
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Usuarios", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuarios", new[] { "Email" });
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Socios", "EstaActivo", c => c.String());
            CreateIndex("dbo.Usuarios", "Email", unique: true);
        }
    }
}

namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablaauxagregada : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntidadAuxiliars",
                c => new
                    {
                        DescuentoPorAntiguedad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TopeAntiguedad = c.Int(nullable: false),
                        DescPorTopeActiv = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TopeActividades = c.Int(nullable: false),
                        ValorMes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorActividad = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.DescuentoPorAntiguedad, t.TopeAntiguedad, t.DescPorTopeActiv, t.TopeActividades, t.ValorMes, t.ValorActividad });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EntidadAuxiliars");
        }
    }
}

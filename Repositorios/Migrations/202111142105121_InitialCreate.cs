namespace Repositorios.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actividades",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        EdadMinima = c.Int(nullable: false),
                        EdadMaxima = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.DiasYHoras",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Dia = c.String(maxLength: 10),
                        Hora = c.Int(nullable: false),
                        CuposMaximos = c.Int(nullable: false),
                        Activ_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actividades", t => t.Activ_Id)
                .Index(t => t.Activ_Id);
            
            CreateTable(
                "dbo.FormasPagos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadActividades = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IngresosActividades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaYHora = c.DateTime(nullable: false),
                        DiaYHr_Id = c.Int(),
                        Soc_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiasYHoras", t => t.DiaYHr_Id)
                .ForeignKey("dbo.Socios", t => t.Soc_Id)
                .Index(t => t.DiaYHr_Id)
                .Index(t => t.Soc_Id);
            
            CreateTable(
                "dbo.Socios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cedula = c.String(nullable: false, maxLength: 10),
                        NombreYapellido = c.String(nullable: false, maxLength: 50),
                        FechaNacimiento = c.DateTime(nullable: false),
                        EstaActivo = c.String(),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PagarMensualidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaPago = c.DateTime(nullable: false),
                        MontoPagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoDescontado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnaFormaPago_Id = c.Int(),
                        UnSocio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormasPagos", t => t.UnaFormaPago_Id)
                .ForeignKey("dbo.Socios", t => t.UnSocio_Id)
                .Index(t => t.UnaFormaPago_Id)
                .Index(t => t.UnSocio_Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 60),
                        Contrasenia = c.String(nullable: false),
                        ContraseniaDesencriptada = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PagarMensualidades", "UnSocio_Id", "dbo.Socios");
            DropForeignKey("dbo.PagarMensualidades", "UnaFormaPago_Id", "dbo.FormasPagos");
            DropForeignKey("dbo.IngresosActividades", "Soc_Id", "dbo.Socios");
            DropForeignKey("dbo.IngresosActividades", "DiaYHr_Id", "dbo.DiasYHoras");
            DropForeignKey("dbo.DiasYHoras", "Activ_Id", "dbo.Actividades");
            DropIndex("dbo.Usuarios", new[] { "Email" });
            DropIndex("dbo.PagarMensualidades", new[] { "UnSocio_Id" });
            DropIndex("dbo.PagarMensualidades", new[] { "UnaFormaPago_Id" });
            DropIndex("dbo.IngresosActividades", new[] { "Soc_Id" });
            DropIndex("dbo.IngresosActividades", new[] { "DiaYHr_Id" });
            DropIndex("dbo.DiasYHoras", new[] { "Activ_Id" });
            DropIndex("dbo.Actividades", new[] { "Nombre" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.PagarMensualidades");
            DropTable("dbo.Socios");
            DropTable("dbo.IngresosActividades");
            DropTable("dbo.FormasPagos");
            DropTable("dbo.DiasYHoras");
            DropTable("dbo.Actividades");
        }
    }
}

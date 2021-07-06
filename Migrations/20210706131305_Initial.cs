using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPNetworkEFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Networks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PrefixLength = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false),
                    Last = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Networks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Networks_Prefix_Last",
                table: "Networks",
                columns: new[] { "Prefix", "Last" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Networks");
        }
    }
}

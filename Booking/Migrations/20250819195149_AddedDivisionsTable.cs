using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class AddedDivisionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Administrators");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionId",
                table: "Events",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionId",
                table: "AtcPositions",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DivisionId",
                table: "AdministratorRole",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Events_DivisionId",
                table: "Events",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_AtcPositions_DivisionId",
                table: "AtcPositions",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministratorRole_DivisionId",
                table: "AdministratorRole",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdministratorRole_Divisions_DivisionId",
                table: "AdministratorRole",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtcPositions_Divisions_DivisionId",
                table: "AtcPositions",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "DivisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Divisions_DivisionId",
                table: "Events",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "DivisionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdministratorRole_Divisions_DivisionId",
                table: "AdministratorRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AtcPositions_Divisions_DivisionId",
                table: "AtcPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Divisions_DivisionId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Events_DivisionId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_AtcPositions_DivisionId",
                table: "AtcPositions");

            migrationBuilder.DropIndex(
                name: "IX_AdministratorRole_DivisionId",
                table: "AdministratorRole");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "AdministratorRole");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionId",
                table: "Events",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionId",
                table: "AtcPositions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DivisionId",
                table: "Administrators",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

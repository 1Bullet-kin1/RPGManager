using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPGManager.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreFactionLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FactionLocations",
                columns: table => new
                {
                    LocationsId = table.Column<int>(type: "INTEGER", nullable: false),
                    PresentFactionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionLocations", x => new { x.LocationsId, x.PresentFactionsId });
                    table.ForeignKey(
                        name: "FK_FactionLocations_Factions_PresentFactionsId",
                        column: x => x.PresentFactionsId,
                        principalTable: "Factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionLocations_Locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactionLocations_PresentFactionsId",
                table: "FactionLocations",
                column: "PresentFactionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactionLocations");
        }
    }
}

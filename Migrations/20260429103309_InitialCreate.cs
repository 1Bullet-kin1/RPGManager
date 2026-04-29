using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPGManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    linked_type = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    linked_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Continents",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    world_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Continents", x => x.id);
                    table.ForeignKey(
                        name: "FK_Continents_Worlds_world_id",
                        column: x => x.world_id,
                        principalTable: "Worlds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    continent_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Regions_Continents_continent_id",
                        column: x => x.continent_id,
                        principalTable: "Continents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    region_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Locations_Regions_region_id",
                        column: x => x.region_id,
                        principalTable: "Regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    alignment = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    base_location_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Factions_Locations_base_location_id",
                        column: x => x.base_location_id,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NPC",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    race = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    @class = table.Column<string>(name: "class", type: "TEXT", maxLength: 50, nullable: true),
                    level = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1),
                    alignment = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    location_id = table.Column<int>(type: "INTEGER", nullable: true),
                    faction_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPC", x => x.id);
                    table.ForeignKey(
                        name: "FK_NPC_Factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "Factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NPC_Locations_location_id",
                        column: x => x.location_id,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NPCRelations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    npc_id_1 = table.Column<int>(type: "INTEGER", nullable: false),
                    npc_id_2 = table.Column<int>(type: "INTEGER", nullable: false),
                    relation_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCRelations", x => x.id);
                    table.ForeignKey(
                        name: "FK_NPCRelations_NPC_npc_id_1",
                        column: x => x.npc_id_1,
                        principalTable: "NPC",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_NPCRelations_NPC_npc_id_2",
                        column: x => x.npc_id_2,
                        principalTable: "NPC",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PinnedNpcs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    npc_id = table.Column<int>(type: "INTEGER", nullable: false),
                    slot = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinnedNpcs", x => x.id);
                    table.ForeignKey(
                        name: "FK_PinnedNpcs_NPC_npc_id",
                        column: x => x.npc_id,
                        principalTable: "NPC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true, defaultValue: "активный"),
                    location_id = table.Column<int>(type: "INTEGER", nullable: true),
                    quest_giver_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.id);
                    table.ForeignKey(
                        name: "FK_Quests_Locations_location_id",
                        column: x => x.location_id,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quests_NPC_quest_giver_id",
                        column: x => x.quest_giver_id,
                        principalTable: "NPC",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuestNPC",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    quest_id = table.Column<int>(type: "INTEGER", nullable: false),
                    npc_id = table.Column<int>(type: "INTEGER", nullable: false),
                    role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestNPC", x => x.id);
                    table.ForeignKey(
                        name: "FK_QuestNPC_NPC_npc_id",
                        column: x => x.npc_id,
                        principalTable: "NPC",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_QuestNPC_Quests_quest_id",
                        column: x => x.quest_id,
                        principalTable: "Quests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Continents_world_id",
                table: "Continents",
                column: "world_id");

            migrationBuilder.CreateIndex(
                name: "IX_Factions_base_location_id",
                table: "Factions",
                column: "base_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_region_id",
                table: "Locations",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_faction_id",
                table: "NPC",
                column: "faction_id");

            migrationBuilder.CreateIndex(
                name: "IX_NPC_location_id",
                table: "NPC",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_NPCRelations_npc_id_1",
                table: "NPCRelations",
                column: "npc_id_1");

            migrationBuilder.CreateIndex(
                name: "IX_NPCRelations_npc_id_2",
                table: "NPCRelations",
                column: "npc_id_2");

            migrationBuilder.CreateIndex(
                name: "IX_PinnedNpcs_npc_id",
                table: "PinnedNpcs",
                column: "npc_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestNPC_npc_id",
                table: "QuestNPC",
                column: "npc_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestNPC_quest_id",
                table: "QuestNPC",
                column: "quest_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_location_id",
                table: "Quests",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_quest_giver_id",
                table: "Quests",
                column: "quest_giver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_continent_id",
                table: "Regions",
                column: "continent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "NPCRelations");

            migrationBuilder.DropTable(
                name: "PinnedNpcs");

            migrationBuilder.DropTable(
                name: "QuestNPC");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "NPC");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Continents");

            migrationBuilder.DropTable(
                name: "Worlds");
        }
    }
}

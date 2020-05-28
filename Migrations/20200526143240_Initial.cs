using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web_fitness.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mail = table.Column<string>(nullable: true),
                    TrainerName = table.Column<string>(nullable: false),
                    TrainerPhone = table.Column<string>(nullable: true),
                    TrainerGender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: false),
                    City = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Target = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    TrainerId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    MeetDate = table.Column<DateTime>(nullable: false),
                    MeetID = table.Column<int>(nullable: false),
                    TrainTypeTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => new { x.MeetDate, x.TrainerId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_Meetings_TrainingTypes_TrainTypeTypeId",
                        column: x => x.TrainTypeTypeId,
                        principalTable: "TrainingTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meetings_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TrainTypeTypeId",
                table: "Meetings",
                column: "TrainTypeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TrainerId",
                table: "Meetings",
                column: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "TrainingTypes");

            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}

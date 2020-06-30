using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web_fitness.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mail = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: false),
                    CustomerPhone = table.Column<string>(nullable: true),
                    CustomerGender = table.Column<string>(nullable: true),
                    CustomerAddress = table.Column<string>(maxLength: 60, nullable: false),
                    CustomerCity = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

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
                    TrainingTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Target = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTypes", x => x.TrainingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingTypeID = table.Column<int>(nullable: false),
                    TrainerID = table.Column<int>(nullable: false),
                    MeetDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetID);
                    table.ForeignKey(
                        name: "FK_Meetings_Trainers_TrainerID",
                        column: x => x.TrainerID,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meetings_TrainingTypes_TrainingTypeID",
                        column: x => x.TrainingTypeID,
                        principalTable: "TrainingTypes",
                        principalColumn: "TrainingTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TrainerID",
                table: "Meetings",
                column: "TrainerID");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TrainingTypeID",
                table: "Meetings",
                column: "TrainingTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "TrainingTypes");
        }
    }
}

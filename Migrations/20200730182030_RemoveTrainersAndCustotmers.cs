using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web_fitness.Migrations
{
    public partial class RemoveTrainersAndCustotmers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingTypeID = table.Column<int>(nullable: false),
                    TrainerID = table.Column<string>(nullable: false),
                    MeetDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetID);
                    table.ForeignKey(
                        name: "FK_Meetings_AspNetUsers_TrainerID",
                        column: x => x.TrainerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meetings_TrainingTypes_TrainingTypeID",
                        column: x => x.TrainingTypeID,
                        principalTable: "TrainingTypes",
                        principalColumn: "TrainingTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.DropTable(
            //    name: "Customers");

            //migrationBuilder.DropTable(
            //    name: "Trainers");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_AspNetUsers_TrainerID",
                table: "Meetings",
                column: "TrainerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_AspNetUsers_TrainerID",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerID",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerAddress = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CustomerCity = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CustomerGender = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerPhone = table.Column<string>(type: "TEXT", nullable: true),
                    Mail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Mail = table.Column<string>(type: "TEXT", nullable: true),
                    TrainerGender = table.Column<string>(type: "TEXT", nullable: true),
                    TrainerName = table.Column<string>(type: "TEXT", nullable: false),
                    TrainerPhone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Mail",
                table: "Trainers",
                column: "Mail",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Trainers_TrainerID",
                table: "Meetings",
                column: "TrainerID",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

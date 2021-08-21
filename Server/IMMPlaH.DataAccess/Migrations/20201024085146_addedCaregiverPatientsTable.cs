using Microsoft.EntityFrameworkCore.Migrations;

namespace IMMPlaH.DataAccess.Migrations
{
    public partial class addedCaregiverPatientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_User_UserId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_User_UserId",
                table: "Patients");

            migrationBuilder.CreateTable(
                name: "CaregiverPatients",
                columns: table => new
                {
                    CaregiverId = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverPatients", x => new { x.CaregiverId, x.PatientId });
                    table.ForeignKey(
                        name: "FK_CaregiverPatients_Caregivers_CaregiverId",
                        column: x => x.CaregiverId,
                        principalTable: "Caregivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaregiverPatients_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverPatients_PatientId",
                table: "CaregiverPatients",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_User_UserId",
                table: "Caregivers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_User_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_User_UserId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_User_UserId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "CaregiverPatients");

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_User_UserId",
                table: "Caregivers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_User_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

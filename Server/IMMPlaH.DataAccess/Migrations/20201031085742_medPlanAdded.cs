using Microsoft.EntityFrameworkCore.Migrations;

namespace IMMPlaH.DataAccess.Migrations
{
    public partial class medPlanAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationPlans",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false),
                    MedicationId = table.Column<int>(nullable: false),
                    IntakeInterval = table.Column<string>(nullable: true),
                    PeriodOfTreatment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationPlans", x => new { x.PlanId, x.MedicationId });
                    table.ForeignKey(
                        name: "FK_MedicationPlans_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationPlans_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPlans_MedicationId",
                table: "MedicationPlans",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PatientId",
                table: "Plans",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationPlans");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}

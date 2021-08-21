using Microsoft.EntityFrameworkCore.Migrations;

namespace IMMPlaH.DataAccess.Migrations
{
    public partial class modelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caregivers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caregivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    SideEffects = table.Column<string>(nullable: true),
                    Dosage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MedicalRecord = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Caregivers",
                columns: new[] { "Id", "Address", "Birthday", "Gender", "Name", "UserId" },
                values: new object[] { 1, "Bucuresti, Str. Victoriei, nr. 10, bl. A1, sc. A, ap. 10", "10/10/1984", "Male", "Adi Mutu", 2 });

            migrationBuilder.InsertData(
                table: "Medications",
                columns: new[] { "Id", "Dosage", "Name", "SideEffects" },
                values: new object[] { 1, 3, "Aspirina", "Diareea, Headache, Stomach pain" });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "Birthday", "Gender", "MedicalRecord", "Name", "UserId" },
                values: new object[] { 1, "Iasi, Str. Principala, nr. 2, bl. D3, sc. C, ap. 12", "05/10/1964", "Male", "Diabet, insomnia and headaches", "Ion Popescu", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caregivers");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}

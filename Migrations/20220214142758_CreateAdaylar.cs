using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewKaratIk.Migrations
{
    public partial class CreateAdaylar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KvkkOnayi = table.Column<bool>(type: "bit", nullable: false),
                    PozisyonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adays_Pozisyons_PozisyonId",
                        column: x => x.PozisyonId,
                        principalTable: "Pozisyons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ozluks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NufusKayitOrnegi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdliSicil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YerlisimYeri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ogrenimBlegesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KanGrubu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    saglikRaporu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NufusCuzdanFotok = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kursBelgeleri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fotograf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskerlikBelgesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    iskurKayit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AileBildirimFormu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isBasvuruFormu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Muvakatname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaasHesapIbanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ozluks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ozluks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeklifFormus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IlaveNot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdayId = table.Column<int>(type: "int", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeklifFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imzaliTeklifFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeklifDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeklifFormus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdayOnaylamas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AdayId = table.Column<int>(type: "int", nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Onay = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdayOnaylamas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdayOnaylamas_Adays_AdayId",
                        column: x => x.AdayId,
                        principalTable: "Adays",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdayOnaylamas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    interviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDone = table.Column<bool>(type: "bit", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ilaveNot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interviews_Adays_AdayId",
                        column: x => x.AdayId,
                        principalTable: "Adays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsTeknik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    interviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterviewUsers_Interviews_interviewId",
                        column: x => x.interviewId,
                        principalTable: "Interviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MulakatDegerlendirmes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mulakId = table.Column<int>(type: "int", nullable: true),
                    Puan = table.Column<double>(type: "float", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdayId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MulakatDegerlendirmes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MulakatDegerlendirmes_Adays_AdayId",
                        column: x => x.AdayId,
                        principalTable: "Adays",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MulakatDegerlendirmes_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MulakatDegerlendirmes_Interviews_mulakId",
                        column: x => x.mulakId,
                        principalTable: "Interviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdayOnaylamas_AdayId",
                table: "AdayOnaylamas",
                column: "AdayId");

            migrationBuilder.CreateIndex(
                name: "IX_AdayOnaylamas_UserId",
                table: "AdayOnaylamas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Adays_PozisyonId",
                table: "Adays",
                column: "PozisyonId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_AdayId",
                table: "Interviews",
                column: "AdayId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewUsers_interviewId",
                table: "InterviewUsers",
                column: "interviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewUsers_UserId",
                table: "InterviewUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MulakatDegerlendirmes_AdayId",
                table: "MulakatDegerlendirmes",
                column: "AdayId");

            migrationBuilder.CreateIndex(
                name: "IX_MulakatDegerlendirmes_mulakId",
                table: "MulakatDegerlendirmes",
                column: "mulakId");

            migrationBuilder.CreateIndex(
                name: "IX_MulakatDegerlendirmes_userId",
                table: "MulakatDegerlendirmes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Ozluks_UserId",
                table: "Ozluks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdayOnaylamas");

            migrationBuilder.DropTable(
                name: "InterviewUsers");

            migrationBuilder.DropTable(
                name: "MulakatDegerlendirmes");

            migrationBuilder.DropTable(
                name: "Ozluks");

            migrationBuilder.DropTable(
                name: "TeklifFormus");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropTable(
                name: "Adays");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MA");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorie",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Magasin",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magasin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appareil",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appareil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appareil_Categorie_CategorieId",
                        column: x => x.CategorieId,
                        principalSchema: "MA",
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abonnement_magasin",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_magasin = table.Column<int>(type: "int", nullable: false),
                    Date_debut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnement_magasin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abonnement_magasin_Magasin_Id_magasin",
                        column: x => x.Id_magasin,
                        principalSchema: "MA",
                        principalTable: "Magasin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paiement",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MagasinId = table.Column<int>(type: "int", nullable: false),
                    NumeroCarte = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    MethodePaiement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paiement_Magasin_MagasinId",
                        column: x => x.MagasinId,
                        principalSchema: "MA",
                        principalTable: "Magasin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecetteMagasin",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecetteTotaleMagasin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NomMagasin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MagasinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetteMagasin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecetteMagasin_Magasin_MagasinId",
                        column: x => x.MagasinId,
                        principalSchema: "MA",
                        principalTable: "Magasin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reparateur",
                schema: "MA",
                columns: table => new
                {
                    reparateurID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<bool>(type: "bit", nullable: false),
                    TaxPer = table.Column<float>(type: "real", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MagasinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparateur", x => x.reparateurID);
                    table.ForeignKey(
                        name: "FK_Reparateur_Magasin_MagasinId",
                        column: x => x.MagasinId,
                        principalSchema: "MA",
                        principalTable: "Magasin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tele = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ReparateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Reparateur_ReparateurId",
                        column: x => x.ReparateurId,
                        principalSchema: "MA",
                        principalTable: "Reparateur",
                        principalColumn: "reparateurID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collegue",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReparateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collegue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collegue_Reparateur_ReparateurId",
                        column: x => x.ReparateurId,
                        principalSchema: "MA",
                        principalTable: "Reparateur",
                        principalColumn: "reparateurID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecetteReparateur",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalReparations = table.Column<float>(type: "real", nullable: false),
                    TotalEmprunts = table.Column<float>(type: "real", nullable: false),
                    RecetteTotale = table.Column<float>(type: "real", nullable: false),
                    TaxReparateur = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReparateurId = table.Column<int>(type: "int", nullable: false),
                    RecetteMagasinId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetteReparateur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecetteReparateur_RecetteMagasin_RecetteMagasinId",
                        column: x => x.RecetteMagasinId,
                        principalSchema: "MA",
                        principalTable: "RecetteMagasin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecetteReparateur_Reparateur_ReparateurId",
                        column: x => x.ReparateurId,
                        principalSchema: "MA",
                        principalTable: "Reparateur",
                        principalColumn: "reparateurID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reparation",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateReception = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdReparateur = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reparation_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "MA",
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reparation_Client_IdClient",
                        column: x => x.IdClient,
                        principalSchema: "MA",
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reparation_Reparateur_IdReparateur",
                        column: x => x.IdReparateur,
                        principalSchema: "MA",
                        principalTable: "Reparateur",
                        principalColumn: "reparateurID");
                });

            migrationBuilder.CreateTable(
                name: "Emprunt",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollegueId = table.Column<int>(type: "int", nullable: false),
                    TypeEmprunt = table.Column<int>(type: "int", nullable: false),
                    Montant = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprunt_Collegue_CollegueId",
                        column: x => x.CollegueId,
                        principalSchema: "MA",
                        principalTable: "Collegue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailsReparation",
                schema: "MA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppareilId = table.Column<int>(type: "int", nullable: false),
                    IMEI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrixTotalReparation = table.Column<float>(type: "real", nullable: false),
                    PrixAvance = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFinEstime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReparationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsReparation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailsReparation_Appareil_AppareilId",
                        column: x => x.AppareilId,
                        principalSchema: "MA",
                        principalTable: "Appareil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailsReparation_Reparation_ReparationId",
                        column: x => x.ReparationId,
                        principalSchema: "MA",
                        principalTable: "Reparation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abonnement_magasin_Id_magasin",
                schema: "MA",
                table: "Abonnement_magasin",
                column: "Id_magasin");

            migrationBuilder.CreateIndex(
                name: "IX_Appareil_CategorieId",
                schema: "MA",
                table: "Appareil",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ReparateurId",
                schema: "MA",
                table: "Client",
                column: "ReparateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Collegue_ReparateurId",
                schema: "MA",
                table: "Collegue",
                column: "ReparateurId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsReparation_AppareilId",
                schema: "MA",
                table: "DetailsReparation",
                column: "AppareilId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsReparation_ReparationId",
                schema: "MA",
                table: "DetailsReparation",
                column: "ReparationId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprunt_CollegueId",
                schema: "MA",
                table: "Emprunt",
                column: "CollegueId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiement_MagasinId",
                schema: "MA",
                table: "Paiement",
                column: "MagasinId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetteMagasin_MagasinId",
                schema: "MA",
                table: "RecetteMagasin",
                column: "MagasinId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetteReparateur_RecetteMagasinId",
                schema: "MA",
                table: "RecetteReparateur",
                column: "RecetteMagasinId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetteReparateur_ReparateurId",
                schema: "MA",
                table: "RecetteReparateur",
                column: "ReparateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Reparateur_MagasinId",
                schema: "MA",
                table: "Reparateur",
                column: "MagasinId");

            migrationBuilder.CreateIndex(
                name: "IX_Reparation_ClientId",
                schema: "MA",
                table: "Reparation",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reparation_IdClient",
                schema: "MA",
                table: "Reparation",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Reparation_IdReparateur",
                schema: "MA",
                table: "Reparation",
                column: "IdReparateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnement_magasin",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "DetailsReparation",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Emprunt",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Paiement",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "RecetteReparateur",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Appareil",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Reparation",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Collegue",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "RecetteMagasin",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Categorie",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Reparateur",
                schema: "MA");

            migrationBuilder.DropTable(
                name: "Magasin",
                schema: "MA");
        }
    }
}

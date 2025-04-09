﻿// <auto-generated />
using System;
using AuthApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthApp.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthApp.Models.Abonnement_magasin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_debut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_expiration")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_magasin")
                        .HasColumnType("int");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Id_magasin");

                    b.ToTable("Abonnement_magasin", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("AuthApp.Models.Appareil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategorieId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategorieId");

                    b.ToTable("Appareil", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Categorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categorie", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReparateurId")
                        .HasColumnType("int");

                    b.Property<int>("Tele")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReparateurId");

                    b.ToTable("Client", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Collegue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroTelephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReparateurId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReparateurId");

                    b.ToTable("Collegue", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.DetailsReparation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppareilId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateFinEstime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IMEI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroSerie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PrixAvance")
                        .HasColumnType("real");

                    b.Property<float>("PrixTotalReparation")
                        .HasColumnType("real");

                    b.Property<int>("ReparationId")
                        .HasColumnType("int");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppareilId");

                    b.HasIndex("ReparationId");

                    b.ToTable("DetailsReparation", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Emprunt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CollegueId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("Montant")
                        .HasColumnType("real");

                    b.Property<int>("TypeEmprunt")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CollegueId");

                    b.ToTable("Emprunt", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Magasin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Patent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Ville")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Magasin", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Paiement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<DateTime>("DatePaiement")
                        .HasColumnType("datetime2");

                    b.Property<int>("MagasinId")
                        .HasColumnType("int");

                    b.Property<string>("MethodePaiement")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NumeroCarte")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("MagasinId");

                    b.ToTable("Paiement", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.RecetteMagasin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("MagasinId")
                        .HasColumnType("int");

                    b.Property<string>("NomMagasin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("RecetteTotaleMagasin")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalTax")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MagasinId");

                    b.ToTable("RecetteMagasin", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.RecetteReparateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RecetteMagasinId")
                        .HasColumnType("int");

                    b.Property<float>("RecetteTotale")
                        .HasColumnType("real");

                    b.Property<int>("ReparateurId")
                        .HasColumnType("int");

                    b.Property<float>("TaxReparateur")
                        .HasColumnType("real");

                    b.Property<float>("TotalEmprunts")
                        .HasColumnType("real");

                    b.Property<float>("TotalReparations")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("RecetteMagasinId");

                    b.HasIndex("ReparateurId");

                    b.ToTable("RecetteReparateur", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Reparateur", b =>
                {
                    b.Property<int>("reparateurID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("reparateurID"));

                    b.Property<int>("MagasinId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Role")
                        .HasColumnType("bit");

                    b.Property<float>("TaxPer")
                        .HasColumnType("real");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("reparateurID");

                    b.HasIndex("MagasinId");

                    b.ToTable("Reparateur", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Reparation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReception")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdReparateur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdReparateur");

                    b.ToTable("Reparation", "MA");
                });

            modelBuilder.Entity("AuthApp.Models.Abonnement_magasin", b =>
                {
                    b.HasOne("AuthApp.Models.Magasin", "Magasin")
                        .WithMany()
                        .HasForeignKey("Id_magasin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Magasin");
                });

            modelBuilder.Entity("AuthApp.Models.Appareil", b =>
                {
                    b.HasOne("AuthApp.Models.Categorie", "Categorie")
                        .WithMany()
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("AuthApp.Models.Client", b =>
                {
                    b.HasOne("AuthApp.Models.Reparateur", "Reparateur")
                        .WithMany()
                        .HasForeignKey("ReparateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reparateur");
                });

            modelBuilder.Entity("AuthApp.Models.Collegue", b =>
                {
                    b.HasOne("AuthApp.Models.Reparateur", "Reparateur")
                        .WithMany()
                        .HasForeignKey("ReparateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reparateur");
                });

            modelBuilder.Entity("AuthApp.Models.DetailsReparation", b =>
                {
                    b.HasOne("AuthApp.Models.Appareil", "Appareil")
                        .WithMany()
                        .HasForeignKey("AppareilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthApp.Models.Reparation", "Reparation")
                        .WithMany("DetailsReparations")
                        .HasForeignKey("ReparationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appareil");

                    b.Navigation("Reparation");
                });

            modelBuilder.Entity("AuthApp.Models.Emprunt", b =>
                {
                    b.HasOne("AuthApp.Models.Collegue", "Collegue")
                        .WithMany("Emprunts")
                        .HasForeignKey("CollegueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collegue");
                });

            modelBuilder.Entity("AuthApp.Models.Paiement", b =>
                {
                    b.HasOne("AuthApp.Models.Magasin", "Magasin")
                        .WithMany()
                        .HasForeignKey("MagasinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Magasin");
                });

            modelBuilder.Entity("AuthApp.Models.RecetteMagasin", b =>
                {
                    b.HasOne("AuthApp.Models.Magasin", "Magasin")
                        .WithMany()
                        .HasForeignKey("MagasinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Magasin");
                });

            modelBuilder.Entity("AuthApp.Models.RecetteReparateur", b =>
                {
                    b.HasOne("AuthApp.Models.RecetteMagasin", "RecetteMagasin")
                        .WithMany("RecettesReparateurs")
                        .HasForeignKey("RecetteMagasinId");

                    b.HasOne("AuthApp.Models.Reparateur", "Reparateur")
                        .WithMany()
                        .HasForeignKey("ReparateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecetteMagasin");

                    b.Navigation("Reparateur");
                });

            modelBuilder.Entity("AuthApp.Models.Reparateur", b =>
                {
                    b.HasOne("AuthApp.Models.Magasin", "Magasin")
                        .WithMany()
                        .HasForeignKey("MagasinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Magasin");
                });

            modelBuilder.Entity("AuthApp.Models.Reparation", b =>
                {
                    b.HasOne("AuthApp.Models.Client", null)
                        .WithMany("Reparations")
                        .HasForeignKey("ClientId");

                    b.HasOne("AuthApp.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AuthApp.Models.Reparateur", "Reparateur")
                        .WithMany("Reparations")
                        .HasForeignKey("IdReparateur")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Reparateur");
                });

            modelBuilder.Entity("AuthApp.Models.Client", b =>
                {
                    b.Navigation("Reparations");
                });

            modelBuilder.Entity("AuthApp.Models.Collegue", b =>
                {
                    b.Navigation("Emprunts");
                });

            modelBuilder.Entity("AuthApp.Models.RecetteMagasin", b =>
                {
                    b.Navigation("RecettesReparateurs");
                });

            modelBuilder.Entity("AuthApp.Models.Reparateur", b =>
                {
                    b.Navigation("Reparations");
                });

            modelBuilder.Entity("AuthApp.Models.Reparation", b =>
                {
                    b.Navigation("DetailsReparations");
                });
#pragma warning restore 612, 618
        }
    }
}

// Data/AdminContext.cs
using Microsoft.EntityFrameworkCore;
using AuthApp.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Collections.Generic;

namespace AuthApp.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reparateur>().ToTable("Reparateur", "MA");
            modelBuilder.Entity<Magasin>().ToTable("Magasin", "MA");
            modelBuilder.Entity<Client>().ToTable("Client", "MA");
            modelBuilder.Entity<Reparation>().ToTable("Reparation", "MA");
            modelBuilder.Entity<DetailsReparation>().ToTable("DetailsReparation", "MA");
            modelBuilder.Entity<Collegue>().ToTable("Collegue", "MA");
            modelBuilder.Entity<Emprunt>().ToTable("Emprunt", "MA");
            modelBuilder.Entity<RecetteReparateur>().ToTable("RecetteReparateur", "MA");
            modelBuilder.Entity<Categorie>().ToTable("Categorie", "MA");
            modelBuilder.Entity<Appareil>().ToTable("Appareil", "MA");
            modelBuilder.Entity<Abonnement_magasin>().ToTable("Abonnement_magasin", "MA");
            modelBuilder.Entity<RecetteMagasin>().ToTable("RecetteMagasin", "MA");
            modelBuilder.Entity<Paiement>().ToTable("Paiement", "MA");
            modelBuilder.Entity<Reparation>()
               .HasOne(r => r.Reparateur)
               .WithMany(r => r.Reparations)
               .HasForeignKey(r => r.IdReparateur)
               .OnDelete(DeleteBehavior.NoAction); // <== désactive la suppression en cascade

            modelBuilder.Entity<Reparation>()
                .HasOne(r => r.Client)
                .WithMany() // si tu n’as pas de navigation inverse
                .HasForeignKey(r => r.IdClient)
                .OnDelete(DeleteBehavior.NoAction);
        }
        
        //cette methode c'est pour creer une colonne dans la table Magasin et reparateur(à modifier ou supprimer)
      
        public DbSet<Magasin> Magasins { get; set; }
        public DbSet<Reparateur> Reparateurs { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Reparation> Reparations { get; set; }
        public DbSet<DetailsReparation> DetailsReparations { get; set; }
        public DbSet<Collegue> Collegues { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }
        public DbSet<RecetteReparateur> RecetteReparateurs { get; set; }

        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Appareil> Appareils { get; set; }

        public DbSet<Abonnement_magasin> Abonnement_Magasins { get; set; }

        public DbSet<RecetteMagasin> RecetteMagasins { get; set; }

        public DbSet<Paiement> Paiements { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemPodatkowy.Models;

namespace SystemPodatkowy.Data
{
    public class SystemPodatkowyContext : DbContext
    {
        public DbSet<Podatnik> Podatnicy { get; set; }
        public DbSet<Wplata> Wplaty { get; set; }
        public DbSet<Deklaracja> Deklaracje { get; set; }
        public DbSet<Powierzchnia> Powierzchnie { get; set; }
        public DbSet<Wspolwlasnosc> Wspolwlasnosci { get; set; }
        public DbSet<StawkaPodatku> StawkiPodatku { get; set; }
        public DbSet<PozycjaDeklaracji> PozycjeDeklaracji { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=SystemPodatkowyBaza;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wspolwlasnosc>()
                .HasKey(w => new { w.PodatnikID, w.PowierzchniaID });

            modelBuilder.Entity<Podatnik>()
                .HasIndex(p => p.NIP)
                .IsUnique();

            modelBuilder.Entity<Podatnik>()
                .HasIndex(p => p.PESEL)
                .IsUnique();

            modelBuilder.Entity<StawkaPodatku>()
                .HasIndex(s => new { s.RokPodatkowy, s.PrzedmiotOpodatkowania })
                .IsUnique()
                .HasDatabaseName("UQ_Stawka_Rok_Przedmiot");

            modelBuilder.Entity<Podatnik>()
                .ToTable(t => t.HasCheckConstraint("CHK_PESEL_Length", "PESEL IS NULL OR (LEN(PESEL) = 11 AND PESEL NOT LIKE '%[^0-9]%')"));
            
            modelBuilder.Entity<Podatnik>()
                .ToTable(t => t.HasCheckConstraint("CHK_NIP_Length", "NIP IS NULL OR (LEN(PESEL) = 10 AND NIP NOT LIKE '%[^0-9]%')"));

            modelBuilder.Entity<Wplata>()
                .ToTable(t => t.HasCheckConstraint("CHK_KwotaWplaty_NonNegative", "KwotaWplaty >= 0"));

            modelBuilder.Entity<Powierzchnia>()
                .ToTable(t => t.HasCheckConstraint("CHK_PowCalkowitaM2_Positive", "PowCalkowitaM2 >= 0"));

            modelBuilder.Entity<Wspolwlasnosc>()
                .ToTable(t => t.HasCheckConstraint("CHK_UdzialProcentowy", "UdziałProcentowy > 0 AND UdziałProcentowy <= 100"));

            modelBuilder.Entity<StawkaPodatku>()
                .ToTable(t => t.HasCheckConstraint("CHK_StawkaZaM2_NonNegative", "StawkaZaM2 >= 0"));

            modelBuilder.Entity<PozycjaDeklaracji>()
                .ToTable(t => t.HasCheckConstraint("CHK_PowierzchniaDoOpod_NonNegative", "PowierzchniaDoOpodatkowaniaM2 >= 0"));
            

            modelBuilder.Entity<PozycjaDeklaracji>()
                .HasOne(p => p.Powierzchnia)
                .WithMany(p => p.Pozycje)
                .HasForeignKey(p => p.PowierzchniaID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PozycjaDeklaracji>()
                .HasOne(p => p.Deklaracja)
                .WithMany(p => p.Pozycje)
                .HasForeignKey(p => p.DeklaracjaID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Wspolwlasnosc>()
                .HasOne(p => p.Powierzchnia)
                .WithMany(p => p.Wspolwlasnosci)
                .HasForeignKey(p => p.PowierzchniaID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

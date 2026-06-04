using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemPodatkowy.Models;
using System.Configuration;

namespace SystemPodatkowy.Data
{
    public class TaxSystemContext : DbContext
    {
        public DbSet<Taxpayer> Taxpayers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TaxDeclaration> Declarations { get; set; }
        public DbSet<PropertyArea> PropertyAreas { get; set; }
        public DbSet<CoOwnership> CoOwnerships { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<DeclarationItem> DeclarationItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TaxSystemDb"].ConnectionString;

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoOwnership>()
                .HasKey(w => new { w.TaxpayerID, w.AreaID });

            modelBuilder.Entity<Taxpayer>()
                .HasIndex(p => p.TaxIdNumber)
                .IsUnique();

            modelBuilder.Entity<Taxpayer>()
                .HasIndex(p => p.PersonalIdNumber)
                .IsUnique();

            modelBuilder.Entity<TaxRate>()
                .HasIndex(s => new { s.TaxYear, s.SubjectOfTaxation })
                .IsUnique()
                .HasDatabaseName("UQ_Stawka_Rok_Przedmiot");

            modelBuilder.Entity<Taxpayer>()
                .ToTable(t => t.HasCheckConstraint("CHK_PESEL_Length", "PESEL IS NULL OR (LEN(PESEL) = 11 AND PESEL NOT LIKE '%[^0-9]%')"));
            
            modelBuilder.Entity<Taxpayer>()
                .ToTable(t => t.HasCheckConstraint("CHK_NIP_Length", "NIP IS NULL OR (LEN(NIP) = 10 AND NIP NOT LIKE '%[^0-9]%')"));

            modelBuilder.Entity<Payment>()
                .ToTable(t => t.HasCheckConstraint("CHK_KwotaWplaty_NonNegative", "KwotaWplaty >= 0"));

            modelBuilder.Entity<PropertyArea>()
                .ToTable(t => t.HasCheckConstraint("CHK_PowCalkowitaM2_Positive", "PowCalkowitaM2 >= 0"));

            modelBuilder.Entity<CoOwnership>()
                .ToTable(t => t.HasCheckConstraint("CHK_UdzialProcentowy", "UdzialProcentowy > 0 AND UdzialProcentowy <= 100"));

            modelBuilder.Entity<TaxRate>()
                .ToTable(t => t.HasCheckConstraint("CHK_StawkaZaM2_NonNegative", "StawkaZaM2 >= 0"));

            modelBuilder.Entity<DeclarationItem>()
                .ToTable(t => t.HasCheckConstraint("CHK_PowierzchniaDoOpod_NonNegative", "PowierzchniaDoOpodatkowaniaM2 >= 0"));
            

            modelBuilder.Entity<DeclarationItem>()
                .HasOne(p => p.PropertyArea)
                .WithMany(p => p.DeclarationItems)
                .HasForeignKey(p => p.AreaID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DeclarationItem>()
                .HasOne(p => p.TaxDeclaration)
                .WithMany(p => p.DeclarationItems)
                .HasForeignKey(p => p.DeclarationID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CoOwnership>()
                .HasOne(p => p.PropertyArea)
                .WithMany(p => p.CoOwnerships)
                .HasForeignKey(p => p.AreaID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

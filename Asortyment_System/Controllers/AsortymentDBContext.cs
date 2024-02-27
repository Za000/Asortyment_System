using Asortyment_System.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asortyment_System.Controllers
{
    public class AsortymentDBContext : DbContext
    {
        public DbSet<Asortyment> Asortyments { get; set; }
        public DbSet<ConnectedEAN> ConnectedEAN { get; set; }
        private NameValueCollection cfg = ConfigurationManager.AppSettings;

        //private configApp cfg = new configApp();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asortyment>()
            .HasKey(p => p.EAN)
                .HasName("PK_EAN");

            modelBuilder.Entity<Asortyment>()
                .Property(r => r.EAN)
                .IsRequired();

            modelBuilder.Entity<ConnectedEAN>()
                .HasKey(p => p.LinkedEAN)
                .HasName("PK_LinkedEAN");

            modelBuilder.Entity<ConnectedEAN>()
                .Property(r => r.LinkedEAN)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer($"{ cfg["ConnectionString"]}; TrustServerCertificate = True; ");
        }

        public bool isProductInDatabase(string EAN)
        {
            var x = this.Asortyments.Where(r => Convert.ToBoolean(r.EAN == EAN));
            if (!x.Any())
                return this.ConnectedEAN.Where(r => Convert.ToBoolean(r.LinkedEAN == EAN)).Any();

            return x.Any();
        }
    }
}

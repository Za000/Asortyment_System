﻿// <auto-generated />
using Asortyment_System.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Asortyment_System.Migrations
{
    [DbContext(typeof(AsortymentDBContext))]
    partial class AsortymentDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Asortyment_System.Controllers.Asortyment", b =>
                {
                    b.Property<string>("EAN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("cena")
                        .HasColumnType("real");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("kodProduktu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nazwaProduktu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("opis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("stanMagazynowy")
                        .HasColumnType("int");

                    b.HasKey("EAN")
                        .HasName("PK_EAN");

                    b.ToTable("Asortyments");
                });

            modelBuilder.Entity("Asortyment_System.Controllers.ConnectedEAN", b =>
                {
                    b.Property<string>("LinkedEAN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AsortymentEAN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BaseEAN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("LinkedEAN")
                        .HasName("PK_LinkedEAN");

                    b.HasIndex("AsortymentEAN");

                    b.ToTable("ConnectedEAN");
                });

            modelBuilder.Entity("Asortyment_System.Controllers.ConnectedEAN", b =>
                {
                    b.HasOne("Asortyment_System.Controllers.Asortyment", null)
                        .WithMany("connected_EAN")
                        .HasForeignKey("AsortymentEAN");
                });

            modelBuilder.Entity("Asortyment_System.Controllers.Asortyment", b =>
                {
                    b.Navigation("connected_EAN");
                });
#pragma warning restore 612, 618
        }
    }
}

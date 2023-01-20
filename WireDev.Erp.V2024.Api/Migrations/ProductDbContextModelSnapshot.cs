﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireDev.Erp.V1.Api.Context;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ProductDb
{
    [DbContext(typeof(ProductDbContext))]
    partial class ProductDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("WireDev.Erp.V1.Models.Storage.Group", b =>
                {
                    b.Property<int>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Used")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            Uuid = 99,
                            Name = "Default_Group",
                            Used = false
                        });
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Storage.Product", b =>
                {
                    b.Property<uint>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Archived")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Availible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("EAN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Group")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prices")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Used")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Uuid = 9999u,
                            Active = false,
                            Archived = false,
                            Availible = 0u,
                            EAN = "[]",
                            Group = 100,
                            Metadata = "{}",
                            Name = "Default_Product",
                            Prices = "[{\"Uuid\":\"66adbf4e-3823-40fd-a26c-a1687d3136cf\",\"Archived\":false,\"Description\":\"Defaul_Price\",\"RetailValue\":10,\"SellValue\":15,\"Locked\":false}]",
                            Properties = "{}",
                            Used = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireDev.Erp.V1.Api.Context;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ApplicationDataDb
{
    [DbContext(typeof(ApplicationDataDbContext))]
    [Migration("20230121144046_sqlite")]
    partial class sqlite
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("WireDev.Erp.V1.Models.Settings", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("NextGroupNumber")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("NextProductNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("99318f06-3486-4c7b-b23e-d013c05573bd"),
                            NextGroupNumber = 100,
                            NextProductNumber = 10000u
                        });
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.DayStats", b =>
                {
                    b.Property<long>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CanceledItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RefundedItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("TEXT");

                    b.Property<uint>("SoldItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date");

                    b.ToTable("DayStats");
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.MonthStats", b =>
                {
                    b.Property<long>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CanceledItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RefundedItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("TEXT");

                    b.Property<uint>("SoldItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date");

                    b.ToTable("MonthStats");
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.ProductStats", b =>
                {
                    b.Property<uint>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Transactions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("ProductStats");
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.TotalStats", b =>
                {
                    b.Property<long>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CanceledItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RefundedItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("TEXT");

                    b.Property<uint>("SoldItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date");

                    b.ToTable("TotalStats");
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.YearStats", b =>
                {
                    b.Property<long>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CanceledItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RefundedItemSells")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("TEXT");

                    b.Property<uint>("SoldItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date");

                    b.ToTable("YearStats");
                });

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

            modelBuilder.Entity("WireDev.Erp.V1.Models.Storage.Price", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Archived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Locked")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("RetailValue")
                        .HasColumnType("decimal(5, 3)");

                    b.Property<decimal>("SellValue")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("Uuid");

                    b.ToTable("Prices");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("d6511f05-7898-4c5a-a145-1dd5ae35caa5"),
                            Archived = false,
                            Description = "Defaul_Price",
                            Locked = false,
                            RetailValue = 10m,
                            SellValue = 15m
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

                    b.Property<int>("Availible")
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
                            Availible = 0,
                            EAN = "[]",
                            Group = 100,
                            Metadata = "{}",
                            Name = "Default_Product",
                            Prices = "[\"d6511f05-7898-4c5a-a145-1dd5ae35caa5\"]",
                            Properties = "{}",
                            Used = false
                        });
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Storage.Purchase", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DatePosted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Items")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Posted")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Uuid");

                    b.ToTable("Purchases");

                    b.HasData(
                        new
                        {
                            Uuid = new Guid("4a0126f3-7fa8-4558-b292-c93ba828b69e"),
                            DatePosted = new DateTime(2023, 1, 21, 14, 40, 46, 831, DateTimeKind.Utc).AddTicks(4240),
                            Items = "[]",
                            Posted = true,
                            TotalPrice = 0m,
                            Type = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
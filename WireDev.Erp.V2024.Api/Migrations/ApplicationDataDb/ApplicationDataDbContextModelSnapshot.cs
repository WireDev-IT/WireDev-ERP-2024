﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireDev.Erp.V1.Api.Context;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ApplicationDataDb
{
    [DbContext(typeof(ApplicationDataDbContext))]
    partial class ApplicationDataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                            Uuid = new Guid("0d556b34-5876-4410-a5d7-632076f9d59a"),
                            NextGroupNumber = 100,
                            NextProductNumber = 10000u
                        });
                });

            modelBuilder.Entity("WireDev.Erp.V1.Models.Statistics.DayStats", b =>
                {
                    b.Property<long>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CanceledItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Expenses")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("PurchasedItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RefundedItems")
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

                    b.Property<uint>("CanceledItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Expenses")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("PurchasedItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RefundedItems")
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

                    b.Property<uint>("CanceledItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Expenses")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("PurchasedItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RefundedItems")
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

                    b.Property<uint>("CanceledItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("DisposedItems")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Expenses")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Losses")
                        .HasColumnType("TEXT");

                    b.Property<uint>("PurchasedItems")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RefundedItems")
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
                            Uuid = new Guid("fd29a134-570b-4d55-90eb-17d0aa76a376"),
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
                            Prices = "[\"fd29a134-570b-4d55-90eb-17d0aa76a376\"]",
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
                            Uuid = new Guid("45ae0228-0d38-44f2-a57a-b228b6254ca8"),
                            DatePosted = new DateTime(2023, 1, 22, 18, 39, 43, 982, DateTimeKind.Utc).AddTicks(8940),
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

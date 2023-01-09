﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireDev.Erp.V1.Api.Context;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ProductDb
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20230109121919_sqlite")]
    partial class sqlite
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("WireDev.Erp.V1.Models.Storage.Product", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Archived")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Availible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

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
                            Uuid = new Guid("2fe4fa87-23b2-47b9-a177-4ea33983aced"),
                            Active = false,
                            Archived = false,
                            Availible = 0u,
                            Categories = "[]",
                            Metadata = "{}",
                            Name = "Default_Product",
                            Prices = "[]",
                            Properties = "{}",
                            Used = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WireDev.Erp.V1.Api.Context;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.StatsDb
{
    [DbContext(typeof(StatsDbContext))]
    partial class StatsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

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
#pragma warning restore 612, 618
        }
    }
}

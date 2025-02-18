﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240119205933_AddedBeaconTrainingData")]
    partial class AddedBeaconTrainingData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DAL.Models.BeaconTrainingData", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int?>("B1")
                        .HasColumnType("int");

                    b.Property<int?>("B10")
                        .HasColumnType("int");

                    b.Property<int?>("B11")
                        .HasColumnType("int");

                    b.Property<int?>("B12")
                        .HasColumnType("int");

                    b.Property<int?>("B13")
                        .HasColumnType("int");

                    b.Property<int?>("B14")
                        .HasColumnType("int");

                    b.Property<int?>("B15")
                        .HasColumnType("int");

                    b.Property<int?>("B16")
                        .HasColumnType("int");

                    b.Property<int?>("B17")
                        .HasColumnType("int");

                    b.Property<int?>("B18")
                        .HasColumnType("int");

                    b.Property<int?>("B19")
                        .HasColumnType("int");

                    b.Property<int?>("B2")
                        .HasColumnType("int");

                    b.Property<int?>("B3")
                        .HasColumnType("int");

                    b.Property<int?>("B4")
                        .HasColumnType("int");

                    b.Property<int?>("B5")
                        .HasColumnType("int");

                    b.Property<int?>("B6")
                        .HasColumnType("int");

                    b.Property<int?>("B7")
                        .HasColumnType("int");

                    b.Property<int?>("B8")
                        .HasColumnType("int");

                    b.Property<int?>("B9")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("BeaconTrainingData");
                });

            modelBuilder.Entity("DLA.Models.Beacon", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<float>("LocationX")
                        .HasColumnType("float");

                    b.Property<float>("LocationY")
                        .HasColumnType("float");

                    b.Property<string>("MacAddress")
                        .HasColumnType("longtext");

                    b.Property<int>("Major")
                        .HasColumnType("int");

                    b.Property<int>("Minor")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("RSSI")
                        .HasColumnType("int");

                    b.Property<int>("RSSI1M")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UUID")
                        .HasColumnType("char(36)");

                    b.HasKey("ID");

                    b.ToTable("Beacons");
                });
#pragma warning restore 612, 618
        }
    }
}

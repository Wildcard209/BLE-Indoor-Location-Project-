﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DAL.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DAL.Models.Beacon", b =>
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

            modelBuilder.Entity("DAL.Models.BeaconTrainingDataX", b =>
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

                    b.Property<int>("LocationX")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("BeaconTrainingDataX");
                });

            modelBuilder.Entity("DAL.Models.BeaconTrainingDataY", b =>
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

                    b.Property<int>("LocationY")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("BeaconTrainingDataY");
                });

            modelBuilder.Entity("DAL.Models.Css", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Css");
                });

            modelBuilder.Entity("DAL.Models.Html", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("HTML");
                });

            modelBuilder.Entity("DAL.Models.Javascript", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Javascript");
                });

            modelBuilder.Entity("DAL.Models.MapInfo", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int?>("BoundX")
                        .HasColumnType("int");

                    b.Property<int?>("BoundY")
                        .HasColumnType("int");

                    b.Property<Guid?>("CurrentCssId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CurrentJsId")
                        .HasColumnType("char(36)");

                    b.Property<int?>("DefaultX")
                        .HasColumnType("int");

                    b.Property<int?>("DefaultY")
                        .HasColumnType("int");

                    b.Property<int?>("HigherX")
                        .HasColumnType("int");

                    b.Property<int?>("HigherY")
                        .HasColumnType("int");

                    b.Property<int?>("ImageHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ImageWidth")
                        .HasColumnType("int");

                    b.Property<int?>("LowerX")
                        .HasColumnType("int");

                    b.Property<int?>("LowerY")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CurrentCssId");

                    b.HasIndex("CurrentJsId");

                    b.ToTable("MapInfo");
                });

            modelBuilder.Entity("DAL.Models.MapPopups", b =>
                {
                    b.Property<Guid>("PopupLocationId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PopupId")
                        .HasColumnType("char(36)");

                    b.HasKey("PopupLocationId", "PopupId");

                    b.HasIndex("PopupId");

                    b.ToTable("MapPopups");
                });

            modelBuilder.Entity("DAL.Models.Popup", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("HtmlId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Popups");
                });

            modelBuilder.Entity("DAL.Models.PopupLocation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("LocationX")
                        .HasColumnType("int");

                    b.Property<int>("LocationY")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("PopupLocation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("23379c47-2571-4807-a0f1-dabb6553e307"),
                            Name = "None",
                            NormalizedName = "NONE"
                        },
                        new
                        {
                            Id = new Guid("f4173b5a-3081-4c87-a335-3ec63841842e"),
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("017a7f87-49fa-40ee-bd5d-5d303d6a7c8d"),
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DAL.Models.MapInfo", b =>
                {
                    b.HasOne("DAL.Models.Css", "CurrentCss")
                        .WithMany()
                        .HasForeignKey("CurrentCssId");

                    b.HasOne("DAL.Models.Javascript", "CurrentJs")
                        .WithMany()
                        .HasForeignKey("CurrentJsId");

                    b.Navigation("CurrentCss");

                    b.Navigation("CurrentJs");
                });

            modelBuilder.Entity("DAL.Models.MapPopups", b =>
                {
                    b.HasOne("DAL.Models.Popup", "Popup")
                        .WithMany()
                        .HasForeignKey("PopupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.PopupLocation", "PopupLocation")
                        .WithMany()
                        .HasForeignKey("PopupLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Popup");

                    b.Navigation("PopupLocation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

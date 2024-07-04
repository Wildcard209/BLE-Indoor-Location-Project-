using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Context: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public Context(DbContextOptions options) : base(options) { }

        public DbSet<Beacon> Beacons { get; set; }
        public DbSet<BeaconTrainingDataX> BeaconTrainingDataX { get; set; }
        public DbSet<BeaconTrainingDataY> BeaconTrainingDataY { get; set; }
        public DbSet<MapInfo> MapInfo { get; set; }
        public DbSet<MapPopups> MapPopups { get; set; }
        public DbSet<Popup> Popups { get; set; }
        public DbSet<PopupLocation> PopupLocation { get; set; }
        public DbSet<Html> HTML { get; set; }
        public DbSet<Javascript> Javascript { get; set; }
        public DbSet<Css> Css { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
               new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "None", NormalizedName = "NONE" },
               new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" }
           );

            modelBuilder.Entity<MapPopups>()
            .HasKey(mp => new { mp.PopupLocationId, mp.PopupId });

            modelBuilder.Entity<MapPopups>()
                .HasOne(mp => mp.PopupLocation)
                .WithMany()
                .HasForeignKey(mp => mp.PopupLocationId);

            modelBuilder.Entity<MapPopups>()
                .HasOne(mp => mp.Popup)
                .WithMany()
                .HasForeignKey(mp => mp.PopupId);

            modelBuilder.Entity<MapInfo>()
            .HasOne(mi => mi.CurrentCss)
            .WithMany()
            .HasForeignKey(mi => mi.CurrentCssId)
            .IsRequired(false);

            modelBuilder.Entity<MapInfo>()
                .HasOne(mi => mi.CurrentJs)
                .WithMany()
                .HasForeignKey(mi => mi.CurrentJsId)
                .IsRequired(false);
        }
    }
}
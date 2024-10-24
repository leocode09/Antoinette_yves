using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WeCareWebApp.Entities;
using WeCareWebApp.Models;

namespace WeCareWebApp.EF
{
    public class WeCareDbContext : DbContext
    {
        public WeCareDbContext(DbContextOptions<WeCareDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Business

            modelBuilder.Entity<BusinessClient>().ToTable(nameof(BusinessClient), "Bzn");

            modelBuilder.Entity<BusinessService>().ToTable(nameof(BusinessService), "Bzn")
                        .HasIndex(i => i.Name).IsUnique();

            modelBuilder.Entity<BusinessService>()
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<Barber>().ToTable(nameof(Barber), "Bzn");
            modelBuilder.Entity<Barber>()
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<Barber>().HasIndex(i => i.Email).IsUnique();
            modelBuilder.Entity<Barber>().HasIndex(i => i.Phone).IsUnique();


            modelBuilder.Entity<Partner>().ToTable(nameof(Partner), "Bzn")
                        .HasIndex(i => i.Name).IsUnique();

            modelBuilder.Entity<Partner>().HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<Partner>().HasIndex(i => i.Email).IsUnique();
            modelBuilder.Entity<Partner>().HasIndex(i => i.MomoCode).IsUnique();
            modelBuilder.Entity<Partner>().HasIndex(i => i.Phone).IsUnique();

            modelBuilder.Entity<Partner>()
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<PartnerService>().ToTable(nameof(PartnerService), "Bzn")
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<PartnerServiceImage>().ToTable(nameof(PartnerServiceImage), "Bzn")
                 .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<Reservation>().ToTable(nameof(Reservation), "Bzn");
            modelBuilder.Entity<ReservationDetail>().ToTable(nameof(ReservationDetail), "Bzn");


            modelBuilder.Entity<ReservationStatus>().ToTable(nameof(ReservationStatus), "Bzn")
                .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            modelBuilder.Entity<TransactionStatus>().ToTable(nameof(TransactionStatus), "Bzn")
                .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<PayTransaction>().ToTable(nameof(PayTransaction), "Bzn");

            #endregion

            #region Security

            modelBuilder.Entity<Role>().ToTable(nameof(Role), "Security").HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<Role>()
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<User>().ToTable(nameof(User), "Security");

            modelBuilder.Entity<User>().HasIndex(i => i.Username).IsUnique();

            modelBuilder.Entity<ClientMovement>().ToTable(nameof(ClientMovement), "Security");

            modelBuilder.Entity<ClientMovement>()
                    .Property(c => c.Id)
                    .ValueGeneratedNever()
                    .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            #endregion

            #region Dto

            modelBuilder.Entity<PartnerDto>().HasNoKey();
            modelBuilder.Entity<PartnerServiceDto>().HasNoKey();
            modelBuilder.Entity<ReservationDto>().HasNoKey();
            modelBuilder.Entity<UserDto>().HasNoKey();
            modelBuilder.Entity<UserAuthenticationDto>().HasNoKey();
            modelBuilder.Entity<PartnerServiceDispDto>().HasNoKey();
            modelBuilder.Entity<ScalarValueDto>().HasNoKey();

            #endregion

        }

        #region Business

        public DbSet<BusinessClient> BusinessClients { get; set; }
        public DbSet<BusinessService> BusinessServices { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerService> PartnerServices { get; set; }
        public DbSet<PartnerServiceImage> PartnerServiceImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationDetail> ReservationDetails { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<PayTransaction> PayTransactions { get; set; }

        #endregion

        #region Security

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClientMovement> ClientMovements { get; set; }

        #endregion

        #region Dto

        public virtual DbSet<PartnerDto> PartnerDtos { get; set; }
        public virtual DbSet<PartnerServiceDto> PartnerServiceDtos { get; set; }
        public virtual DbSet<ReservationDto> ReservationDtos { get; set; }
        public virtual DbSet<UserDto> UserDtos { get; set; }
        public virtual DbSet<UserAuthenticationDto> UserAuthenticationDtos { get; set; }
        public virtual DbSet<PartnerServiceDispDto> PartnerServiceDispDtos { get; set; }
        public virtual DbSet<ScalarValueDto> ScalarValueDtos { get; set; }

        #endregion

    }
}

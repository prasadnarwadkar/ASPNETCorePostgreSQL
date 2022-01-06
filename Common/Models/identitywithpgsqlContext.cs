using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Common.Models
{
    public partial class identitywithpgsqlContext : DbContext
    {
        public identitywithpgsqlContext()
        {
        }

        public identitywithpgsqlContext(DbContextOptions<identitywithpgsqlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Unnaturaldeaths> Unnaturaldeaths { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=identitywithpgsql;Username=postgres;Password=Tetya1:2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasColumnType("timestamp with time zone");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasIdentityOptions(null, null, null, 1000000000L, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("character varying(500)[]");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying(100)[]");

                entity.Property(e => e.Uhid)
                    .HasColumnName("UHID")
                    .HasComment("Unique health ID");
            });

            modelBuilder.Entity<Unnaturaldeaths>(entity =>
            {
                entity.ToTable("unnaturaldeaths");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.CidOrPassport)
                    .HasColumnName("cidOrPassport")
                    .HasMaxLength(50);

                entity.Property(e => e.DateOfPostmortemExamination)
                    .HasColumnName("dateOfPostmortemExamination")
                    .HasColumnType("date");

                entity.Property(e => e.DeceasedName)
                    .HasColumnName("deceasedName")
                    .HasMaxLength(50);

                entity.Property(e => e.Dzongkhag)
                    .HasColumnName("dzongkhag")
                    .HasMaxLength(50);

                entity.Property(e => e.GeneralExternalInformation)
                    .HasColumnName("generalExternalInformation")
                    .HasMaxLength(300);

                entity.Property(e => e.History)
                    .HasColumnName("history")
                    .HasMaxLength(300);

                entity.Property(e => e.ImformantCidNo)
                    .HasColumnName("imformantCidNo")
                    .HasMaxLength(50);

                entity.Property(e => e.InformantName)
                    .HasColumnName("informantName")
                    .HasMaxLength(50);

                entity.Property(e => e.InformantRelationToDeceased)
                    .HasColumnName("informantRelationToDeceased")
                    .HasMaxLength(50);

                entity.Property(e => e.Isactive)
                    .IsRequired()
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Lastchanged)
                    .HasColumnName("lastchanged")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Nationality)
                    .HasColumnName("nationality")
                    .HasMaxLength(50);

                entity.Property(e => e.PlaceOfExamination)
                    .HasColumnName("placeOfExamination")
                    .HasMaxLength(50);

                entity.Property(e => e.PoliceCaseNo).HasColumnName("policeCaseNo");

                entity.Property(e => e.PoliceStation)
                    .HasColumnName("policeStation")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(256);

                entity.Property(e => e.SceneOfDeath)
                    .HasColumnName("sceneOfDeath")
                    .HasMaxLength(200);

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasMaxLength(5);

                entity.Property(e => e.TimeOfPostmortemExamination)
                    .HasColumnName("timeOfPostmortemExamination")
                    .HasColumnType("time(6) with time zone");

                entity.Property(e => e.Transactedby)
                    .IsRequired()
                    .HasColumnName("transactedby")
                    .HasMaxLength(50);

                entity.Property(e => e.Transacteddate)
                    .HasColumnName("transacteddate")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Gdpr.Domain.Models
{
    public partial class GdprCore22Context : DbContext
    {
        public GdprCore22Context()
        {
        }

        public GdprCore22Context(DbContextOptions<GdprCore22Context> options)
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
        public virtual DbSet<CaptureOutputLog> CaptureOutputLog { get; set; }
        public virtual DbSet<GdprCxp> GdprCxp { get; set; }
        public virtual DbSet<GdprDcd> GdprDcd { get; set; }
        public virtual DbSet<GdprDtd> GdprDtd { get; set; }
        public virtual DbSet<GdprDxp> GdprDxp { get; set; }
        public virtual DbSet<GdprEdt> GdprEdt { get; set; }
        public virtual DbSet<GdprFpd> GdprFpd { get; set; }
        public virtual DbSet<GdprPdf> GdprPdf { get; set; }
        public virtual DbSet<GdprPds> GdprPds { get; set; }
        public virtual DbSet<GdprRpd> GdprRpd { get; set; }
        public virtual DbSet<GdprRxp> GdprRxp { get; set; }
        public virtual DbSet<GdprTest> GdprTest { get; set; }
        public virtual DbSet<GdprUrd> GdprUrd { get; set; }
        public virtual DbSet<GdprUta> GdprUta { get; set; }
        public virtual DbSet<GdprWst> GdprWst { get; set; }
        public virtual DbSet<GdprWxr> GdprWxr { get; set; }
        public virtual DbSet<PrivateConfigurations> PrivateConfigurations { get; set; }
        public virtual DbSet<PrivateNewTestClassList> PrivateNewTestClassList { get; set; }
        public virtual DbSet<PrivateRenamedObjectLog> PrivateRenamedObjectLog { get; set; }
        public virtual DbSet<TestResult> TestResult { get; set; }

        // Unable to generate entity type for table 'tSQLt.Private_NullCellTable'. Please see the warning messages.
        // Unable to generate entity type for table 'tSQLt.Private_AssertEqualsTableSchema_Actual'. Please see the warning messages.
        // Unable to generate entity type for table 'tSQLt.Private_AssertEqualsTableSchema_Expected'. Please see the warning messages.
        // Unable to generate entity type for table 'tSQLt.TestMessage'. Please see the warning messages.
        // Unable to generate entity type for table 'tSQLt.Run_LastExecution'. Please see the warning messages.
        // Unable to generate entity type for table 'tSQLt.Private_ExpectException'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GdprCore22;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

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
                entity.HasIndex(e => e.Email)
                    .HasName("EmailIndex")
                    .IsUnique();

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<CaptureOutputLog>(entity =>
            {
                entity.ToTable("CaptureOutputLog", "tSQLt");
            });

            modelBuilder.Entity<GdprCxp>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprCXP__3214EC062F5272BC")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprCXP");

                entity.HasIndex(e => e.FpdId)
                    .IsUnique();

                entity.HasIndex(e => e.Granted)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => e.RpdId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Granted).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Fpd)
                    .WithOne(p => p.GdprCxp)
                    .HasForeignKey<GdprCxp>(d => d.FpdId)
                    .HasConstraintName("FK_GdprCXP_FpdId");

                entity.HasOne(d => d.Rpd)
                    .WithOne(p => p.GdprCxp)
                    .HasForeignKey<GdprCxp>(d => d.RpdId)
                    .HasConstraintName("FK_GdprCXP_RpdId");
            });

            modelBuilder.Entity<GdprDcd>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprDCD__3214EC062DABE583")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprDCD");

                entity.HasIndex(e => e.DisplayName)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<GdprDtd>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprDTD__3214EC06688279ED")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprDTD");

                entity.HasIndex(e => e.DisplayName)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<GdprDxp>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprDXP__3214EC061B61D0F3")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprDXP");

                entity.HasIndex(e => e.FpdId)
                    .IsUnique();

                entity.HasIndex(e => e.PdsId)
                    .HasName("IX_GdprDXP_RpdId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Fpd)
                    .WithOne(p => p.GdprDxp)
                    .HasForeignKey<GdprDxp>(d => d.FpdId)
                    .HasConstraintName("FK_GdprDXP_FpdId");

                entity.HasOne(d => d.Pds)
                    .WithOne(p => p.GdprDxp)
                    .HasForeignKey<GdprDxp>(d => d.PdsId)
                    .HasConstraintName("FK_GdprDXP_PdsId");
            });

            modelBuilder.Entity<GdprEdt>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprEDT__3214EC0623F49DCC")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprEDT");

                entity.HasIndex(e => e.FpdId)
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Country).HasMaxLength(40);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.HasOne(d => d.Fpd)
                    .WithOne(p => p.GdprEdt)
                    .HasForeignKey<GdprEdt>(d => d.FpdId)
                    .HasConstraintName("FK_GdprEDT_FpdId");
            });

            modelBuilder.Entity<GdprFpd>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprFPD__3214EC06C771A155")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprFPD");

                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<GdprPdf>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprPDF__3214EC06113E4EDF")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprPDF");

                entity.HasIndex(e => e.Value)
                    .HasName("UQ__GdprPDF__07D9BBC28DD440D0")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.FieldName).HasMaxLength(128);

                entity.Property(e => e.TableName).HasMaxLength(128);
            });

            modelBuilder.Entity<GdprPds>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprPDS__3214EC063DD5A1B9")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprPDS");

                entity.HasIndex(e => e.DcdId)
                    .IsUnique();

                entity.HasIndex(e => e.DisplayName)
                    .HasName("IX_GdprPDS_Name")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => e.DtdId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.FieldName).HasMaxLength(128);

                entity.Property(e => e.TableName).HasMaxLength(128);

                entity.HasOne(d => d.Dcd)
                    .WithOne(p => p.GdprPds)
                    .HasForeignKey<GdprPds>(d => d.DcdId)
                    .HasConstraintName("FK_GdprPDS_DcdId");

                entity.HasOne(d => d.Dtd)
                    .WithOne(p => p.GdprPds)
                    .HasForeignKey<GdprPds>(d => d.DtdId)
                    .HasConstraintName("FK_GdprPDS_DtdId");
            });

            modelBuilder.Entity<GdprRpd>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprRPD__3214EC06C939E235")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprRPD");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => e.UrdId)
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.NotificationUrl).HasColumnName("NotificationURL");

                entity.HasOne(d => d.Urd)
                    .WithOne(p => p.GdprRpd)
                    .HasForeignKey<GdprRpd>(d => d.UrdId)
                    .HasConstraintName("FK_GdprRPD_UrdId");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.GdprRpd)
                    .HasForeignKey<GdprRpd>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GdprRPD_UserId");
            });

            modelBuilder.Entity<GdprRxp>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprRXP__3214EC06452F9549")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprRXP");

                entity.HasIndex(e => e.FpdId)
                    .IsUnique();

                entity.HasIndex(e => e.UrdId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Fpd)
                    .WithOne(p => p.GdprRxp)
                    .HasForeignKey<GdprRxp>(d => d.FpdId)
                    .HasConstraintName("FK_GdprRXP_FpdId");

                entity.HasOne(d => d.Urd)
                    .WithOne(p => p.GdprRxp)
                    .HasForeignKey<GdprRxp>(d => d.UrdId)
                    .HasConstraintName("FK_GdprRXP_UrdId");
            });

            modelBuilder.Entity<GdprTest>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprTest__3214EC06B468FCA7")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Created)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Ninumber).HasColumnName("NINumber");
            });

            modelBuilder.Entity<GdprUrd>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprURD__3214EC068F5C713B")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprURD");

                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => e.RoleId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.HasOne(d => d.Role)
                    .WithOne(p => p.GdprUrd)
                    .HasForeignKey<GdprUrd>(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GdprURD_RoleId");
            });

            modelBuilder.Entity<GdprUta>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprUTA__3214EC061B73E524")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprUTA");

                entity.HasIndex(e => e.Accepted)
                    .HasName("IX_GdprUTA_Created")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => e.RpdId)
                    .IsUnique();

                entity.HasIndex(e => e.WstId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Accepted).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Rpd)
                    .WithOne(p => p.GdprUta)
                    .HasForeignKey<GdprUta>(d => d.RpdId)
                    .HasConstraintName("FK_GdprUTA_RpdId");

                entity.HasOne(d => d.Wst)
                    .WithOne(p => p.GdprUta)
                    .HasForeignKey<GdprUta>(d => d.WstId)
                    .HasConstraintName("FK_GdprUTA_WstId");
            });

            modelBuilder.Entity<GdprWst>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprWST__3214EC061F1D8825")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprWST");

                entity.HasIndex(e => e.Title)
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Hash).HasMaxLength(250);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<GdprWxr>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__GdprWXR__3214EC064D388937")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("GdprWXR");

                entity.HasIndex(e => e.UrdId)
                    .IsUnique();

                entity.HasIndex(e => e.WstId)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Urd)
                    .WithOne(p => p.GdprWxr)
                    .HasForeignKey<GdprWxr>(d => d.UrdId)
                    .HasConstraintName("FK_GdprWXR_UrdId");

                entity.HasOne(d => d.Wst)
                    .WithOne(p => p.GdprWxr)
                    .HasForeignKey<GdprWxr>(d => d.WstId)
                    .HasConstraintName("FK_GdprWXR_WstId");
            });

            modelBuilder.Entity<PrivateConfigurations>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Private___737584F72FFFBB67");

                entity.ToTable("Private_Configurations", "tSQLt");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnType("sql_variant");
            });

            modelBuilder.Entity<PrivateNewTestClassList>(entity =>
            {
                entity.HasKey(e => e.ClassName)
                    .HasName("PK__Private___F8BF561A03A13E55");

                entity.ToTable("Private_NewTestClassList", "tSQLt");

                entity.Property(e => e.ClassName).ValueGeneratedNever();
            });

            modelBuilder.Entity<PrivateRenamedObjectLog>(entity =>
            {
                entity.ToTable("Private_RenamedObjectLog", "tSQLt");

                entity.Property(e => e.OriginalName).IsRequired();
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.ToTable("TestResult", "tSQLt");

                entity.Property(e => e.Class).IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(517)
                    .HasComputedColumnSql("((quotename([Class])+'.')+quotename([TestCase]))");

                entity.Property(e => e.TestCase).IsRequired();

                entity.Property(e => e.TestEndTime).HasColumnType("datetime");

                entity.Property(e => e.TestStartTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TranName).IsRequired();
            });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BankManagement.Models.EntityModels
{
    public partial class BankDBContext : DbContext
    {
        public BankDBContext()
        {
        }

        public BankDBContext(DbContextOptions<BankDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-DCACMRHC\\MSSQLSERVER02;Database=BankDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.AccountUsername, "UQ__Accounts__EA401835437270ED")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AccountUsername)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Balance).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.BankId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.HolderName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__Accounts__BankId__151B244E");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.BankId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiffImps)
                    .HasColumnType("decimal(5, 4)")
                    .HasColumnName("DiffIMPS");

                entity.Property(e => e.DiffRtgs)
                    .HasColumnType("decimal(5, 4)")
                    .HasColumnName("DiffRTGS");

                entity.Property(e => e.SameImps)
                    .HasColumnType("decimal(5, 4)")
                    .HasColumnName("SameIMPS");

                entity.Property(e => e.SameRtgs)
                    .HasColumnType("decimal(5, 4)")
                    .HasColumnName("SameRTGS");

                entity.Property(e => e.StaffPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StaffUsername)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupportedCurrencies).HasColumnType("text");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode)
                    .HasName("PK__Currenci__408426BE4548D752");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(19, 4)");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.Amount).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.RecipientAccountId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SenderAccountId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.RecipientAccount)
                    .WithMany(p => p.TransactionRecipientAccounts)
                    .HasForeignKey(d => d.RecipientAccountId)
                    .HasConstraintName("FK__Transacti__Recip__18EBB532");

                entity.HasOne(d => d.SenderAccount)
                    .WithMany(p => p.TransactionSenderAccounts)
                    .HasForeignKey(d => d.SenderAccountId)
                    .HasConstraintName("FK__Transacti__Sende__17F790F9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System.Data.Entity;

#nullable disable

namespace BankManagement.Services.EntityModels
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
///# warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-DCACMRHC\\MSSQLSERVER02;Database=BankDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.AccountUsername, "UQ__Accounts__EA4018355A979B9C")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsFixedLength(true);

                entity.Property(e => e.AccountUsername)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Balance).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.BankId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HolderName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__Accounts__BankId__656C112C");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(e => e.StaffLogin, "UQ__Banks__5124817F411D92CA")
                    .IsUnique();

                entity.Property(e => e.BankId)
                    .HasMaxLength(20)
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

                entity.Property(e => e.StaffLogin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StaffPassword)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsFixedLength(true);

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
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Amount).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.AssociatedAccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TransactionAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Transacti__Accou__68487DD7");

                entity.HasOne(d => d.AssociatedAccount)
                    .WithMany(p => p.TransactionAssociatedAccounts)
                    .HasForeignKey(d => d.AssociatedAccountId)
                    .HasConstraintName("FK__Transacti__Assoc__693CA210");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

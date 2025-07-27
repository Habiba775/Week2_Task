using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using week2_Task.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


using week2_Task.Models.Entities.Enums;
using week2_Task.Models;

namespace week2_Task.Data
{
    public class ApplicationDBContext : IdentityDbContext<APPUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Merchant Entity
            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(m => m.Email)
                      .IsRequired();

                entity.HasIndex(m => m.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Merchants_Email_Unique");

                entity.Property(m => m.Phone)
                      .HasMaxLength(20);

                entity.Property(m => m.Address)
                      .HasMaxLength(200);

                entity.Property(m => m.CreatedDate)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // Payment Entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.Property(p => p.Amount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Currency)
                      .IsRequired();

                entity.Property(p => p.Method)
                      .IsRequired();

                entity.Property(p => p.Status)
                      .HasDefaultValue(Status.Pending)
                      .HasConversion<int>()
                      .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(p => p.CreatedDate)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(p => p.Merchant)
                      .WithMany(m => m.Payments)
                      .HasForeignKey(p => p.MerchantId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.MerchantId);
            });

            // Transaction Entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TransactionReference)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(t => t.TransactionReference)
                      .IsUnique();

                entity.Property(t => t.Description)
                      .HasMaxLength(500);

                entity.Property(t => t.ProcessedAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(t => t.TransactionDate)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(t => t.Notes)
                      .HasMaxLength(1000);

                entity.HasOne(t => t.Payment)
                      .WithMany(p => p.Transactions)
                      .HasForeignKey(t => t.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(t => t.PaymentId);
            });

            // User Entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(u => u.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Email_Unique");
            });

            // Final call
            base.OnModelCreating(modelBuilder);
        }
    }
}


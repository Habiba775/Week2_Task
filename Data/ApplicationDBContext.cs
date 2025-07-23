using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using week2_Task.Models.Entities;
using week2_Task.Models.Entities.Enums;
namespace week2_Task.Data

{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    ;

                entity.Property(m => m.Email)

                    .IsRequired()
                    ;
                modelBuilder.Entity<Merchant>()
    .HasIndex(m => m.Email)
    .IsUnique()
    .HasDatabaseName("IX_Merchants_Email_Unique");

                entity.Property(m => m.Phone)
                    .HasMaxLength(20);

                entity.Property(m => m.Address)
                    .HasMaxLength(200);

                entity.Property(m => m.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            });



            /* entity.HasCheckConstraint("CK_Merchant_Email_Format",
                 "[Email] LIKE '%@%.%'"); */



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
                    .HasDefaultValue(Status.Pending); ;

                entity.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(p => p.Status)
    .HasDefaultValue(Status.Pending)
    .HasConversion<int>()
    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

                modelBuilder.Entity<Payment>()
     .HasOne(p => p.Merchant)
     .WithMany(m => m.Payments)
     .HasForeignKey(p => p.MerchantId)
     .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.MerchantId);
            });
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
                    .HasDefaultValueSql("GETUTCDATE()"); ;

                entity.Property(t => t.Notes)
                    .HasMaxLength(1000);

                modelBuilder.Entity<Transaction>()
     .HasOne(t => t.Payment)
     .WithMany(p => p.Transactions)
     .HasForeignKey(t => t.PaymentId)
     .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(t => t.PaymentId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}






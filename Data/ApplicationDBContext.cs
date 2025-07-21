using Microsoft.EntityFrameworkCore;
using week2_Task.Models.Entities;

namespace week2_Task.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }






        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(m => m.Email)
                      .IsRequired();

                entity.HasIndex(m => m.Email)
                      .IsUnique();

                entity.HasMany(m => m.Payments)
                      .WithOne(p => p.Merchant)
                      .HasForeignKey(p => p.MerchantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Amount)
                      .HasPrecision(18, 2);

                entity.Property(p => p.Currency)
                      .HasMaxLength(3)
                      .IsRequired();
            });
        }
    }
}


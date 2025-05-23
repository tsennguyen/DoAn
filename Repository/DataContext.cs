using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shopping_Laptop.Models;

namespace Shopping_Laptop.Repository
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for all models
            modelBuilder.Entity<MomoInfoModel>()
                .Property(m => m.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetails>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.ShippingCost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.OldPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Rating)
                .HasColumnType("decimal(3,2)");

            modelBuilder.Entity<ShippingModel>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StatisticalModel>()
                .Property(s => s.revenue)
                .HasColumnType("decimal(18,2)");

            // Configure indexes
            modelBuilder.Entity<ProductModel>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            modelBuilder.Entity<ProductModel>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<CategoryModel>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<BrandModel>()
                .HasIndex(b => b.Slug)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<StatisticalModel> Statistical { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<WishlistModel> Wishlist { get; set; }
        public DbSet<CompareModel> Compare { get; set; }
        public DbSet<ProductQuantityModel> ProductQuantities { get; set; }
        public DbSet<ShippingModel> Shippings { get; set; }
        public DbSet<CouponModel> Coupons { get; set; }
        public DbSet<MomoInfoModel> MomoInfos { get; set; }
        public DbSet<UserBehaviorModel> UserBehaviors { get; set; }
    }
}

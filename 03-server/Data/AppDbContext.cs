using Microsoft.EntityFrameworkCore;
using XtxServer.Entities;

namespace XtxServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Goods> Goods { get; set; }
    public DbSet<Sku> Skus { get; set; }
    public DbSet<Spec> Specs { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<HotRecommend> HotRecommends { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Mobile).IsUnique();
            entity.HasIndex(e => e.Account).IsUnique();
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Goods>(entity =>
        {
            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Sku>(entity =>
        {
            entity.HasOne(e => e.Goods)
                .WithMany(e => e.Skus)
                .HasForeignKey(e => e.GoodsId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Spec>(entity =>
        {
            entity.HasOne(e => e.Goods)
                .WithMany(e => e.Specs)
                .HasForeignKey(e => e.GoodsId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Sku)
                .WithMany()
                .HasForeignKey(e => e.SkuId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(e => e.Order)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

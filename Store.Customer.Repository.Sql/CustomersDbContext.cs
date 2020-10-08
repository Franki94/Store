using Microsoft.EntityFrameworkCore;
using Store.Customer.Models;

namespace Store.Customer.Repository.Sql
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("pk_customers");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id").ValueGeneratedNever();

                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(30);

                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(30);

                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(30);

                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(25);

                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(25);
            });

            modelBuilder.Entity<Models.Carts>(entity =>
            {
                entity.HasKey(e => e.CartId).HasName("pk_carts");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(e => e.CustomerNavigation)
                .WithOne(e => e.CarNavigation)
                .HasForeignKey<Carts>(e => e.CustomerId).HasConstraintName("fk_carts__customer");
            });

            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.HasKey(e => e.CartItemId).HasName("pk_cart_items");

                entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasColumnType("money");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.HasOne(e => e.CartNavigation)
                .WithMany(e => e.CartItemsNavigation)
                .HasForeignKey(e => e.CartId)
                .HasConstraintName("fk_cart_items__cart");

                entity.HasOne(e => e.ProductNavigation)
                .WithMany(e => e.CartItemsNavigation)
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("fk_cart_items__product");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("pk_products");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("money");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });
        }

    }
}

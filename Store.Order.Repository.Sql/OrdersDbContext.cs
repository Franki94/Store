using Microsoft.EntityFrameworkCore;
using Store.Order.Models;

namespace Store.Order.Repository.Sql
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Models.Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<OrderStatuses> OrderStatuses { get; set; }

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
            });

            modelBuilder.Entity<Models.Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("pk_orders");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("money");

                entity.Property(e => e.Currency).HasColumnName("currency").IsRequired().HasMaxLength(5);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(e => e.OrderStatusNavigation)
                .WithMany(e => e.OrdersNavigation)
                .HasForeignKey(e => e.StatusId).HasConstraintName("fk_orders__order_status");

                entity.HasOne(e => e.CustomerNavigation)
                .WithMany(e => e.OrdersNavigation)
                .HasForeignKey(e => e.CustomerId).HasConstraintName("fk_orders__customers");
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.OrderItemId).HasName("pk_order_items");

                entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price").HasColumnType("money");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasColumnType("money");

                entity.HasOne(e => e.OrderNavigation)
                .WithMany(e => e.OrderItemsNavigation)
                .HasForeignKey(e => e.OrderId)
                .HasConstraintName("fk_orders__order_item");
            });

            modelBuilder.Entity<OrderStatuses>(entity =>
            {
                entity.HasKey(e => e.OrderStatusId).HasName("pk_order_statuses");

                entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");

                entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasMaxLength(50);
            });
        }
    }
}

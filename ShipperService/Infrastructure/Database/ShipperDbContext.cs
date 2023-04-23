using Microsoft.EntityFrameworkCore;
using ShipperService.Domain.Entities;
using ShipperService.Infrastructure.Models.ShippingOrders;

namespace ShipperService.Infrastructure.Database
{
    public class ShipperDbContext : DbContext
    {
        public const string ShipperSchema = "ms_shipper";
        public const string ShippingSchema = "ms_shipping";

        public ShipperDbContext(DbContextOptions<ShipperDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<ShipperSession> ShipperSessions { get; set; }
        public virtual DbSet<ShippingOrder> ShippingOrders { get; set; }
        public virtual DbSet<ShippingOrderLog> ShippingOrderLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var shipperSessionTableBuider = modelBuilder.Entity<ShipperSession>().ToTable("ShipperSessions", ShipperSchema);
            shipperSessionTableBuider
                .Property(p => p.EntityVersion)
                .HasColumnName("RowVersion")
                .IsConcurrencyToken();

            var shippingOrderTableBuilder = modelBuilder.Entity<ShippingOrder>().ToTable("ShippingOrders", ShippingSchema);
            modelBuilder.HasSequence<long>("ShippingOrderIdSeq").IncrementsBy(5);
            shippingOrderTableBuilder
                .Property(p => p.Id)
                .UseHiLo("ShippingOrderIdSeq", ShippingSchema);
            shippingOrderTableBuilder
                .Property(p => p.EntityVersion)
                .HasColumnName("RowVersion")
                .IsConcurrencyToken();
            shippingOrderTableBuilder
                .Property(p => p.FromLatitude)
                .HasColumnType("decimal(18,15)");
            shippingOrderTableBuilder
                .Property(p => p.FromLongitude)
                .HasColumnType("decimal(18,15)");
            shippingOrderTableBuilder
                .Property(p => p.ToLatitude)
                .HasColumnType("decimal(18,15)");
            shippingOrderTableBuilder
                .Property(p => p.ToLongitude)
                .HasColumnType("decimal(18,15)");

            var orderLogTableBuilder = modelBuilder.Entity<ShippingOrderLog>().ToTable("ShippingOrderLogs", ShippingSchema);
            orderLogTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();
        }
    }
}

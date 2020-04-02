using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace inventory_management_api.Models
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DeliveryDetailInfo> DeliveryDetailInfo { get; set; }
        public virtual DbSet<DeliveryMainInfo> DeliveryMainInfo { get; set; }
        public virtual DbSet<InventoryInfo> InventoryInfo { get; set; }
        public virtual DbSet<OutboundDetailInfo> OutboundDetailInfo { get; set; }
        public virtual DbSet<OutboundMainInfo> OutboundMainInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryDetailInfo>(entity =>
            {
                entity.HasKey(e => e.DetailId);

                entity.ToTable("delivery_detail_info");

                entity.Property(e => e.DetailId).HasColumnName("detail_id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.OrderNumber)
                    .IsRequired()
                    .HasColumnName("order_number")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductSpec)
                    .IsRequired()
                    .HasColumnName("product_spec")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DeliveryMainInfo>(entity =>
            {
                entity.HasKey(e => e.DeliveryOrderNumber);

                entity.ToTable("delivery_main_info");

                entity.Property(e => e.DeliveryOrderNumber)
                    .HasColumnName("delivery_order_number")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("date");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InventoryInfo>(entity =>
            {
                entity.ToTable("inventory_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductSpec)
                    .IsRequired()
                    .HasColumnName("product_spec")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<OutboundDetailInfo>(entity =>
            {
                entity.HasKey(e => e.DetailId);

                entity.ToTable("outbound_detail_info");

                entity.Property(e => e.DetailId).HasColumnName("detail_id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.OrderNumber)
                    .IsRequired()
                    .HasColumnName("order_number")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductSpec)
                    .IsRequired()
                    .HasColumnName("product_spec")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OutboundMainInfo>(entity =>
            {
                entity.HasKey(e => e.OutboundOrderNumber);

                entity.ToTable("outbound_main_info");

                entity.Property(e => e.OutboundOrderNumber)
                    .HasColumnName("outbound_order_number")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OutboundDate)
                    .HasColumnName("outbound_date")
                    .HasColumnType("date");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

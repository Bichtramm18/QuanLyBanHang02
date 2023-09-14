using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class QLNHoaContext : DbContext
    {
        public QLNHoaContext()
        {
        }

        public QLNHoaContext(DbContextOptions<QLNHoaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Perfume> Perfumes { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=QLNHoa;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.MaKh);

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.HoTenKh)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("HoTenKH");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.MaDh)
                    .HasName("PK__Orders__27258661FF44B953");

                entity.Property(e => e.MaDh)
                    .ValueGeneratedNever()
                    .HasColumnName("MaDH");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.NgayDat).HasColumnType("date");

                entity.Property(e => e.ThanhToan)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__MaKH__5070F446");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.MaDh, e.MaNh })
                    .HasName("PK__OrderDet__2725866115614171");

                entity.Property(e => e.MaDh).HasColumnName("MaDH");

                entity.Property(e => e.MaNh).HasColumnName("MaNH");

                entity.Property(e => e.GiamGia).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.MaDhNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MaDh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDetai__MaDH__534D60F1");

                entity.HasOne(d => d.MaNhNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MaNh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDetai__MaNH__5441852A");
            });

            modelBuilder.Entity<Perfume>(entity =>
            {
                entity.HasKey(e => e.MaNh)
                    .HasName("PK__Perfumes__2725D738958D366D");

                entity.Property(e => e.MaNh).HasColumnName("MaNH");

                entity.Property(e => e.Huong).HasMaxLength(50);

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.TenNh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TenNH");

                entity.HasOne(d => d.MaNccNavigation)
                    .WithMany(p => p.Perfumes)
                    .HasForeignKey(d => d.MaNcc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Perfumes__MaNCC__3D5E1FD2");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.MaNcc);

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.QuocGia)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);

                entity.Property(e => e.TenNcc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TenNCC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

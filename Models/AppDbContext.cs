using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<HangLaptop> HangLaptops { get; set; }
        public DbSet<HinhAnhLaptop> HinhAnhLaptops { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NhanVien> Nhanviens { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Các cấu hình Fluent API (định nghĩa khóa, quan hệ) có thể thêm tại đây
        }
    }
}

using BLL;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;

namespace API_Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ==========================================
            // ĐĂNG KÝ DEPENCY INJECTION (DI)
            // ==========================================
            
            // 1. Đăng ký Data Access Layer (DAL - Repositories)
            builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
            builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
            builder.Services.AddScoped<ILaptopRepository, LaptopRepository>();
            builder.Services.AddScoped<IGioHangRepository, GioHangRepository>();
            builder.Services.AddScoped<IDonHangRepository, DonHangRepository>();
            builder.Services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
            builder.Services.AddScoped<IPhieuNhapRepository, PhieuNhapRepository>();
            builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
            builder.Services.AddScoped<INhaCungCapRepository, NhaCungCapRepository>();

            // 2. Đăng ký Business Logic Layer (BLL - Services)
            builder.Services.AddScoped<ITaiKhoanBLL, TaiKhoanBLL>();
            builder.Services.AddScoped<IKhachHangBLL, KhachHangBLL>();
            builder.Services.AddScoped<ILaptopBLL, LaptopBLL>();
            builder.Services.AddScoped<IgioHangBLL, giohangBLL>();
            builder.Services.AddScoped<IDonHangBLL, DonHangBLL>();
            builder.Services.AddScoped<IDanhGiaBLL, DanhGiaBLL>();
            builder.Services.AddScoped<IPhieuNhapBLL, PhieuNhapBLL>();
            builder.Services.AddScoped<INhanVienBLL, NhanVienBLL>();
            builder.Services.AddScoped<INhaCungCapBLL, NhaCungCapBLL>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

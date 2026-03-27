using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Thêm file cấu hình Ocelot.json (Bắt buộc)
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Đăng ký dịch vụ Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();

// Kích hoạt Ocelot Middleware
app.UseOcelot().Wait();

app.Run();

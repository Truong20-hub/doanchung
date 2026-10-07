using AutoMapper;
using BLL.Interfaces;
using BLL.Mappings;
using BLL.Services;
using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối và thông tin JWT
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

// Add services
builder.Services.AddControllers();

// CẤU HÌNH CORS CHUẨN
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8081",
                "http://localhost:19006",
                "http://10.40.1.187:8081",
                "http://10.40.1.187:5259",
                "http://localhost:3000"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging()   // hiện cả giá trị dữ liệu
           .LogTo(Console.WriteLine, LogLevel.Information));
// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Authorization
builder.Services.AddAuthorization();

// Dependency Injection - Repositories & Services
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<IVaiTroService, VaiTroService>();

builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddScoped<INguoiDungService, NguoiDungService>();

builder.Services.AddScoped<IGiaoVienRepository, GiaoVienRepository>();
builder.Services.AddScoped<IGiaoVienService, GiaoVienService>();

builder.Services.AddScoped<IHocVienRepository, HocVienRepository>();
builder.Services.AddScoped<IHocVienService, HocVienService>();

builder.Services.AddScoped<IKhoaHocRepository, KhoaHocRepository>();
builder.Services.AddScoped<IKhoaHocService, KhoaHocService>();

builder.Services.AddScoped<IPhongHocRepository, PhongHocRepository>();
builder.Services.AddScoped<IPhongHocService, PhongHocService>();

builder.Services.AddScoped<ILopHocRepository, LopHocRepository>();
builder.Services.AddScoped<ILopHocService, LopHocService>();

builder.Services.AddScoped<IBuoiHocRepository, BuoiHocRepository>();
builder.Services.AddScoped<IBuoiHocService, BuoiHocService>();

builder.Services.AddScoped<IDangKyHocRepository, DangKyHocRepository>();
builder.Services.AddScoped<IDangKyHocService, DangKyHocService>();

builder.Services.AddScoped<ILichHocRepository, LichHocRepository>();
builder.Services.AddScoped<ILichHocService, LichHocService>();

builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();

builder.Services.AddScoped<IChiTietHoaDonRepository, ChiTietHoaDonRepository>();
builder.Services.AddScoped<IChiTietHoaDonService, ChiTietHoaDonService>();

builder.Services.AddScoped<ITinNhanRepository, TinNhanRepository>();
builder.Services.AddScoped<ITinNhanService, TinNhanService>();

builder.Services.AddScoped<IThongBaoRepository, ThongBaoRepository>();
builder.Services.AddScoped<IThongBaoService, ThongBaoService>();

builder.Services.AddScoped<IBaoCaoRepository, BaoCaoRepository>();
builder.Services.AddScoped<IBaoCaoService, BaoCaoService>();

builder.Services.AddScoped<IThanhToanRepository, ThanhToanRepository>();
builder.Services.AddScoped<IThanhToanService, ThanhToanService>();

builder.Services.AddScoped<IKyThiRepository, KyThiRepository>();
builder.Services.AddScoped<IKyThiService, KyThiService>();

builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();

builder.Services.AddScoped<IDiemThiRepository, DiemThiRepository>();
builder.Services.AddScoped<IDiemThiService, DiemThiService>();

builder.Services.AddScoped<IBaiTapRepository, BaiTapRepository>();
builder.Services.AddScoped<IDiemRepository, DiemRepository>();
builder.Services.AddScoped<IBaiTapService, BaiTapService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

Directory.CreateDirectory(
    builder.Environment.WebRootPath ??
    Path.Combine(builder.Environment.ContentRootPath, "wwwroot"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. BẬT CORS TRƯỚC HẾT TẤT CẢ CÁC MIDDLEWARE
app.UseCors("Frontend");

// 2. KHÔNG DÙNG HTTPS REDIRECTION KHI DEV VỚI EXPO WEB / MOBILE
// app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        if (context.File.PhysicalPath?.Contains(
                "UploadedAssignments",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            context.Context.Response.Headers["Content-Disposition"] =
                "attachment";
            context.Context.Response.Headers["X-Content-Type-Options"] =
                "nosniff";
        }
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
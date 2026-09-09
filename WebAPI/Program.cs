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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        if (origins is { Length: > 0 })
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        connectionString);
});

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

// Dependency Injection
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<IVaiTroService, VaiTroService>();

builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddScoped<INguoiDungService, NguoiDungService>();

builder.Services.AddScoped<IGiaoVienRepository, GiaoVienRepository>();
builder.Services.AddScoped<IGiaoVienService, GiaoVienService>();

// HocVien
builder.Services.AddScoped<IHocVienRepository, HocVienRepository>();
builder.Services.AddScoped<IHocVienService, HocVienService>();
// KhoaHoc
builder.Services.AddScoped<IKhoaHocRepository, KhoaHocRepository>();
builder.Services.AddScoped<IKhoaHocService, KhoaHocService>();
// phong hoc
builder.Services.AddScoped<IPhongHocRepository, PhongHocRepository>();
builder.Services.AddScoped<IPhongHocService, PhongHocService>();
// lop hoc
builder.Services.AddScoped<ILopHocRepository, LopHocRepository>();
builder.Services.AddScoped<ILopHocService, LopHocService>();
// buoi hoc
builder.Services.AddScoped<IBuoiHocRepository, BuoiHocRepository>();
builder.Services.AddScoped<IBuoiHocService, BuoiHocService>();
// Đăng ký học
builder.Services.AddScoped<IDangKyHocRepository, DangKyHocRepository>();
builder.Services.AddScoped<IDangKyHocService, DangKyHocService>();
// LichHoc
builder.Services.AddScoped<ILichHocRepository, LichHocRepository>();
builder.Services.AddScoped<ILichHocService, LichHocService>();
// HoaDon
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
// Chi tiết hóa đơn
builder.Services.AddScoped<IChiTietHoaDonRepository, ChiTietHoaDonRepository>();
builder.Services.AddScoped<IChiTietHoaDonService, ChiTietHoaDonService>();
// Tin nhắn
builder.Services.AddScoped<ITinNhanRepository, TinNhanRepository>();
builder.Services.AddScoped<ITinNhanService, TinNhanService>();
// Thông báo
builder.Services.AddScoped<IThongBaoRepository, ThongBaoRepository>();
builder.Services.AddScoped<IThongBaoService, ThongBaoService>();
// Báo cáo
builder.Services.AddScoped<IBaoCaoRepository, BaoCaoRepository>();
builder.Services.AddScoped<IBaoCaoService, BaoCaoService>();
// thanh toán
builder.Services.AddScoped<IThanhToanRepository, ThanhToanRepository>();
builder.Services.AddScoped<IThanhToanService, ThanhToanService>();
// KyThi
builder.Services.AddScoped<IKyThiRepository, KyThiRepository>();
builder.Services.AddScoped<IKyThiService, KyThiService>();
// Diem danh 
builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();
// điểm thi
builder.Services.AddScoped<IDiemThiRepository, DiemThiRepository>();
builder.Services.AddScoped<IDiemThiService, DiemThiService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
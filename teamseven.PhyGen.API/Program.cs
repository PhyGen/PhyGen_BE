using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Services.Services.Authentication;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Repository.Interfaces;
using teamseven.PhyGen.Services.Services.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

// ================= CẤU HÌNH DB =================
builder.Services.AddDbContext<teamsevenphygendbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ================= CẤU HÌNH AUTHENTICATION =================
ConfigureAuthentication(builder.Services, builder.Configuration);




// ================= ĐĂNG KÝ REPOSITORY & SERVICE =================
// Đăng ký dịch vụ với DI container

// GHI CHÚ: Đăng ký Scoped và các lifetime trong ASP.NET Core
// 1. Scoped: Một instance mỗi HTTP request, dùng cho DbContext, repository, service liên quan đến request
//    - Ví dụ: DbContext, GenericRepository<Image>, ImageService
//    - Lý do: Đảm bảo nhất quán trong request, an toàn với nhiều request đồng thời


// 📌 Repository Layer (Scoped)
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserSocialProviderRepository,UserSocialProviderRepository>();

// 📌 Service Layer (Scoped)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
// 📌 Utility & Helper Services
builder.Services.AddTransient<IEmailService, EmailService>(); // Email service (Transient)
builder.Services.AddSingleton<IPasswordEncryptionService, PasswordEncryptionService>(); // Encryption (Singleton)



// 2. Singleton: Một instance duy nhất cho cả ứng dụng, dùng cho dịch vụ không trạng thái
//    - Ví dụ: Cấu hình, logger toàn cục
//    - Cẩn thận: Không dùng cho DbContext/repository vì gây lỗi concurrency
// Ví dụ: builder.Services.AddSingleton<SomeConfigService>();

// 3. Transient: Instance mới mỗi lần gọi, dùng cho dịch vụ nhẹ, không lưu trạng thái
//    - Ví dụ: Email sender, dịch vụ tạm thời
// Ví dụ: builder.Services.AddTransient<SomeLightweightService>();

//TÓM LẠI: NÊN HỎI CON AI COI NÊN XÀI SCOPE GÌ???






// ================= CẤU HÌNH BLOB STORAGE =================
////var blobServiceClient = new BlobServiceClient(builder.Configuration["AzureStorage:ConnectionString"]);
//builder.Services.AddSingleton(blobServiceClient);

// ================= CẤU HÌNH CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// ================= THAY ĐỔI ĐỂ LẮNG NGHE HTTP =================
builder.WebHost.UseUrls("http://0.0.0.0:5000"); // Đã thay đổi từ https sang http

// ================= CẤU HÌNH SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhyGen", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập Bearer Token (VD: Bearer eyJhbGciOi...)",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.EnableAnnotations();
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ================= CẤU HÌNH AUTOMAPPER =================


// ================= CẤU HÌNH CONTROLLERS =================
builder.Services.AddControllers();

var app = builder.Build();

// ================= MIDDLEWARE =================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhyGen API V1");
    c.RoutePrefix = "swagger";
});

app.UseDatabaseKeepAlive();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ================= AUTHENTICATION CONFIGURATION FUNCTION =================
void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
{
    // Retrieve JWT key from configuration
    var jwtKey = config["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JWT Key is missing in the configuration.");
    }

    // JWT Authentication Configuration
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Verify the issuer
            ValidateAudience = true, // Verify the audience
            ValidateLifetime = true, // Check expiration time
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT Token validated successfully.");
                return Task.CompletedTask;
            }
        };
    });


    // Google Authentication
    services.AddAuthentication()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = config["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];
            googleOptions.SaveTokens = true;
            googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            googleOptions.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        });


    // Authorization policies
    services.AddAuthorization(options =>
    {
        options.AddPolicy("DeliveringStaffPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        options.AddPolicy("SaleStaffPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "staff"));
    });
}
using GorevY.Data;
using GorevY.Middlewares;
using GorevY.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
app.UseMiddleware<ExceptionMiddleware>();


var builder = WebApplication.CreateBuilder(args);

// CORS ayarlar�n� g�ncelle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://example.com", "https://anotherexample.com") // Sadece bu domainlerden gelen isteklere izin ver
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Veritaban� ayarlar�
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Maksimum 5 kez dene
                maxRetryDelay: TimeSpan.FromSeconds(10), // 10 saniyelik gecikme ile
                errorNumbersToAdd: null); // Belirli hata kodlar� ile s�n�rlama yapma
        }));

// JWT ayarlar�
var key = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(key))
{
    throw new InvalidOperationException("JWT anahtar� yap�land�r�lmam��.");
}

var keyBytes = Encoding.UTF8.GetBytes(key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true, // Token s�resi dolmu�sa reddet
        ClockSkew = TimeSpan.Zero // Token s�resi do�rulama s�ras�nda esnek olmayacak
    };
});

// Servisleri ekle
builder.Services.AddScoped<KullaniciService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Orta katmanlar� ekle
app.UseCors("AllowSpecificOrigins"); // Yeni CORS politikas�n� kullan
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>(); // �zel middleware'i burada kullan�yoruz
app.MapControllers();

// Swagger yap�land�rmas�
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger"; // "https://localhost:5260/swagger" yolunda a��l�r
});

app.Run();

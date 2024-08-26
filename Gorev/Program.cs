using GorevY.Data;
using GorevY.Middlewares;
using GorevY.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS ayarlar�n� ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
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
    options.RequireHttpsMetadata = false; // Geli�tirme ortam�nda ge�ici olarak kullanabilirsiniz
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});

// Servisleri ekle
builder.Services.AddScoped<KullaniciService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Orta katmanlar� ekle
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Swagger yap�land�rmas�
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>(); // �zel middleware'i burada kullan�yoruz
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = "swagger"; // Ana sayfada de�il, "https://localhost:7257/swagger" yolunda a��l�r
    });
}
if (app.Environment.IsProduction())
{
    app.UseWhen(context => context.User.IsInRole("Admin"), appBuilder =>
    {
        appBuilder.UseSwagger();
        appBuilder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            c.RoutePrefix = "swagger";
        });
    });
}

app.Run();

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration configuration)
    {
        // JWT yap�land�rma de�erlerini al ve null kontrol� yap
        _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(_key), "JWT Key configuration is missing");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(_issuer), "JWT Issuer configuration is missing");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(_audience), "JWT Audience configuration is missing");
    }

    public string GenerateToken(int userId)
    {
        // E�er _key bo� ya da null ise, hata f�rlat
        if (string.IsNullOrEmpty(_key))
        {
            throw new InvalidOperationException("JWT Key is not configured.");
        }

        // Anahtar� byte dizisine d�n��t�r
        var keyBytes = Encoding.UTF8.GetBytes(_key);

        // JWT Token olu�turucu
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // Token'a kullan�c� ID'sini claim olarak ekle
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
            // Token ge�erlilik s�resi (1 saat)
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,  // Yap�land�rma dosyas�ndaki Issuer de�eri
            Audience = _audience,  // Yap�land�rma dosyas�ndaki Audience de�eri
            // Anahtar ile token'� imzala
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        // Token olu�tur
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);  // Token'� string olarak geri d�nd�r
    }
}

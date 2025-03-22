using Microsoft.IdentityModel.Tokens;
using project6._1Api.Entities;
using project6._1Api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtService
{
    private readonly databaseContext _dbContext;
    private readonly IConfiguration _configuration;

    public JwtService(databaseContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> Authenticate(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var userAccount = _dbContext.User.FirstOrDefault(x => x.Username == request.Username);
        if (userAccount == null || string.IsNullOrEmpty(userAccount.Password) || userAccount.Password != HashString(request.Password))
            return null;

        var accessToken = GenerateToken(userAccount.Username);
        var refreshToken = GenerateRefreshToken();

        userAccount.RefreshToken = refreshToken;
        userAccount.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays"));
        await _dbContext.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = userAccount.Username,
            ExpiresIn = 30* 60
        };
    }

    public async Task<LoginResponse?> RefreshToken(string refreshToken)
    {
        var userAccount = _dbContext.User.FirstOrDefault(x => x.RefreshToken == refreshToken);

        if (userAccount is null || userAccount.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        var accessToken = GenerateToken(userAccount.Username);
        var newRefreshToken = GenerateRefreshToken();

        userAccount.RefreshToken = newRefreshToken;
        userAccount.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays"));
        _dbContext.SaveChanges();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            Username = userAccount.Username,
            ExpiresIn = 30 * 60
        };
    }

    private string GenerateToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1800),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public static string HashString(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}

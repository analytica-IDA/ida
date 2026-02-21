using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public interface IAuthService
    {
        string GenerateToken(Usuario usuario);
        bool ValidatePassword(string inputSenha, string storedSenha);
        string HashPassword(string senha);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "vY8v7mX9qR2e5n2z4bA7wE1c0sP3jL5k");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login),
                    new Claim("id", usuario.Id.ToString()),
                    new Claim("roleId", usuario.Cargo?.IdRole.ToString() ?? "0"),
                    new Claim("idCliente", usuario.Pessoa?.IdCliente?.ToString() ?? "0")
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidatePassword(string inputSenha, string storedSenha)
        {
            // Fallback for older plaintext passwords just in case, otherwise verify hash
            if (!storedSenha.StartsWith("$2a$") && !storedSenha.StartsWith("$2b$") && !storedSenha.StartsWith("$2y$"))
            {
                return inputSenha == storedSenha;
            }
            return BCrypt.Net.BCrypt.Verify(inputSenha, storedSenha);
        }

        public string HashPassword(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
    }
}

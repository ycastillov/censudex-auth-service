using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using censudex_auth_service.Src.Helpers;
using censudex_auth_service.Src.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace censudex_auth_service.Src.Services
{
    /// <summary>
    /// Servicio para la generación de tokens JWT.
    /// </summary>
    /// <typeparam name="JwtSettings">Configuración de JWT.</typeparam>
    public class TokenService(
        IOptions<JwtSettings> jwtOptions,
        TokenValidationParameters validationParameters,
        ILogoutService logoutService
    ) : ITokenService
    {
        // Configuración de JWT inyectada
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        private readonly TokenValidationParameters _validationParameters = validationParameters;
        private readonly ILogoutService _logoutService = logoutService;

        public string GenerateToken(Guid userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(
                    token,
                    _validationParameters,
                    out var validatedToken
                );

                // Extraer jti (puede venir como claim "jti" o JwtRegisteredClaimNames.Jti)
                var jti =
                    principal.FindFirst("jti")?.Value
                    ?? principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (string.IsNullOrEmpty(jti))
                    return null;

                // Comprobar blocklist (LogoutService espera jti, no token completo)
                if (_logoutService.IsTokenBlocked(jti))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

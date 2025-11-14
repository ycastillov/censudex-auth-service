using System.Collections.Concurrent;
using censudex_auth_service.Src.Interfaces;

namespace censudex_auth_service.Src.Services
{
    /// <summary>
    /// Servicio para el cierre de sesión y bloqueo de tokens JWT.
    /// </summary>
    public class LogoutService : ILogoutService
    {
        // Almacén en memoria para tokens bloqueados
        private readonly ConcurrentDictionary<string, DateTime> _blockedTokens = new();

        public void BlockToken(string jti, DateTime expiresAt)
        {
            _blockedTokens[jti] = expiresAt;
        }

        public bool IsTokenBlocked(string jti)
        {
            if (_blockedTokens.TryGetValue(jti, out var expiresAt))
            {
                if (expiresAt > DateTime.UtcNow)
                {
                    return true;
                }
                else
                {
                    _blockedTokens.TryRemove(jti, out _);
                }
            }
            return false;
        }
    }
}

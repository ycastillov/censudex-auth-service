using System.Collections.Concurrent;
using censudex_auth_service.Src.Interfaces;
using StackExchange.Redis;

namespace censudex_auth_service.Src.Services
{
    /// <summary>
    /// Servicio para el cierre de sesión y bloqueo de tokens JWT.
    /// </summary>
    public class LogoutService(IConnectionMultiplexer redisDatabase) : ILogoutService
    {
        private readonly IDatabase _redisDatabase = redisDatabase.GetDatabase();
        private const string Prefix = "blk:jti:";

        public void BlockToken(string jti, DateTime expiresAt)
        {
            var key = Prefix + jti;
            var ttl = expiresAt - DateTime.UtcNow;
            if (ttl <= TimeSpan.Zero)
                ttl = TimeSpan.FromSeconds(1);
            _redisDatabase.StringSet(key, "1", ttl);
        }

        public bool IsTokenBlocked(string jti)
        {
            var key = Prefix + jti;
            return _redisDatabase.KeyExists(key);
        }
    }
}

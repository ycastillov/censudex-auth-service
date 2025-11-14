namespace censudex_auth_service.Src.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de cierre de sesión.
    /// </summary>
    public interface ILogoutService
    {
        /// <summary>
        /// Bloquea un token JWT para evitar su uso futuro.
        /// </summary>
        /// <param name="jti">Identificador único del token JWT.</param>
        /// <param name="expiresAt">Fecha y hora de expiración del token.</param>
        void BlockToken(string jti, DateTime expiresAt);

        /// <summary>
        /// Verifica si un token JWT está bloqueado.
        /// </summary>
        /// <param name="jti">Identificador único del token JWT.</param>
        /// <returns>True si el token está bloqueado, de lo contrario false.</returns>
        bool IsTokenBlocked(string jti);
    }
}

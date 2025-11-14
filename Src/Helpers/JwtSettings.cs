namespace censudex_auth_service.Src.Helpers
{
    /// <summary>
    /// Opciones de configuración para JWT (JSON Web Tokens).
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Clave secreta utilizada para firmar el token JWT.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Emisor del token JWT.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audiencia del token JWT.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Tiempo de expiración del token JWT en minutos.
        /// </summary>
        public int ExpiresInMinutes { get; set; }
    }
}

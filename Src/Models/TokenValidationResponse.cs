namespace censudex_auth_service.Src.Models
{
    /// <summary>
    /// Modelo para la respuesta de validación de token JWT.
    /// </summary>
    public class TokenValidationResponse
    {
        /// <summary>
        /// Indica si el token es válido.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Identificador del usuario asociado al token.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Rol del usuario asociado al token.
        /// </summary>
        public string? Role { get; set; }
    }
}

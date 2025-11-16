namespace censudex_auth_service.Src.DTOs
{
    /// <summary>
    /// DTO para la respuesta de validación de token.
    /// </summary>
    public class ValidateTokenResponseDTO
    {
        /// <summary>
        /// Indica si el token es válido o no.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Identificador único del usuario asociado al token.
        /// </summary>
        public string? UserId { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario asociado al token.
        /// </summary>
        public string? Role { get; set; } = string.Empty;

        /// <summary>
        /// Mensaje adicional relacionado con la validación del token.
        /// </summary>
        public string? Message { get; set; } = string.Empty;
        public bool Valid { get; internal set; }
    }
}

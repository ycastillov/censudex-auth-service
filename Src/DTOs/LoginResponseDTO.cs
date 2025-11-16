namespace censudex_auth_service.Src.DTOs
{
    /// <summary>
    /// DTO para la respuesta de inicio de sesión.
    /// </summary>
    public class LoginResponseDTO
    {
        /// <summary>
        /// Token de autenticación generado tras el inicio de sesión.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Identificador único del usuario que ha iniciado sesión.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario que ha iniciado sesión.
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}

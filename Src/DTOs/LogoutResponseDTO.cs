namespace censudex_auth_service.Src.DTOs
{
    /// <summary>
    /// DTO para la respuesta de cierre de sesión.
    /// </summary>
    public class LogoutResponseDto
    {
        /// <summary>
        /// Mensaje relacionado con el cierre de sesión.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}

namespace censudex_auth_service.Src.DTOs
{
    /// <summary>
    /// DTO para la solicitud de inicio de sesión.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Nombre de usuario o correo electrónico del usuario.
        /// </summary>
        public string UsernameOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}

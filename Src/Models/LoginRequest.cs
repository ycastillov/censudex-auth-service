namespace censudex_auth_service.Src.Models
{
    /// <summary>
    /// Modelo para la solicitud de inicio de sesión.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Identificar del ussuario (puede ser nombre de usuario o correo electrónico).
        /// </summary>
        public string Identifier { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}

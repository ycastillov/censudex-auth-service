namespace censudex_auth_service.Src.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de generación de tokens JWT.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Genera un token JWT para un usuario dado.
        /// </summary>
        /// <param name="userId">El identificador único del usuario.</param>
        /// <param name="role">El rol del usuario.</param>
        /// <returns>Un token JWT como cadena.</returns>
        string GenerateToken(Guid userId, string role);
    }
}

namespace censudex_auth_service.Src.Models
{
    /// <summary>
    /// Modelo para la respuesta de token JWT.
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

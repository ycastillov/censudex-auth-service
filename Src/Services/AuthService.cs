using System.IdentityModel.Tokens.Jwt;
using censudex_auth_service.Src.DTOs;
using censudex_auth_service.Src.Interfaces;
using static ClientsService.Grpc.ClientsGrpc;

namespace censudex_auth_service.Src.Services
{
    /// <summary>
    /// Servicio para la autenticación de usuarios.
    /// </summary>
    /// <param name="clientsGrpcClient">Cliente gRPC para servicios de clientes.</param>
    /// <param name="tokenService">Servicio para la gestión de tokens.</param>
    /// <param name="logoutService">Servicio para la gestión de cierre de sesión.</param>
    public class AuthService(
        ClientsGrpcClient clientsGrpcClient,
        ITokenService tokenService,
        ILogoutService logoutService
    ) : IAuthService
    {
        private readonly ClientsGrpcClient _clientsGrpcClient = clientsGrpcClient;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ILogoutService _logoutService = logoutService;

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
        {
            try
            {
                // Buscar cliente por nombre de usuario o correo electrónico
                var clientsResponse = _clientsGrpcClient.GetClientByIdentifier(
                    new() { Identifier = loginRequest.UsernameOrEmail }
                );

                if (clientsResponse == null)
                {
                    return await Task.FromResult<LoginResponseDTO?>(null);
                }

                // Obtener hash y comparar

                // Convertir id (string) a Guid si GenerateToken espera Guid
                if (!Guid.TryParse(clientsResponse.Id, out var clientGuid))
                {
                    // id inválido
                    return await Task.FromResult<LoginResponseDTO?>(null);
                }

                var token = _tokenService.GenerateToken(clientGuid, "customer");

                return new LoginResponseDTO
                {
                    Token = token,
                    UserId = clientGuid.ToString(),
                    Role = "customer",
                };
            }
            catch (Exception)
            {
                // Manejo de errores (registro, rethrow, etc.)
                throw;
            }
        }

        public bool Logout(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // jti en jwt.Id o claim "jti"
                var jti = jwt.Id ?? jwt.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
                if (string.IsNullOrEmpty(jti))
                    return false;

                // exp (unix seconds) -> DateTime UTC; fallback a jwt.ValidTo
                DateTime expiresAt;
                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (long.TryParse(expClaim, out var expUnix))
                {
                    expiresAt = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                }
                else
                {
                    expiresAt = jwt.ValidTo;
                }

                _logoutService.BlockToken(jti, expiresAt);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

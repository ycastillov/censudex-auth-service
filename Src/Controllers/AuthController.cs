using censudex_auth_service.Src.DTOs;
using censudex_auth_service.Src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace censudex_auth_service.Src.Controllers
{
    /// <summary>
    /// Controlador para la gestión de autenticación.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ITokenService tokenService)
        : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Inicia sesión con las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginRequest">DTO que contiene las credenciales de inicio de sesión.</param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(loginRequestDTO);

                if (response == null)
                {
                    return Unauthorized(new { Message = "Credenciales inválidas." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new { Message = $"Error al procesar la solicitud: {ex.Message}" }
                );
            }
        }

        /// <summary>
        /// Valida el token JWT proporcionado en la cabecera Authorization.
        /// </summary>
        /// <returns>Indica si el token es válido o no.</returns>
        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                return Unauthorized(
                    new ValidateTokenResponseDTO { IsValid = false, Message = "Token faltante" }
                );

            var principal = _tokenService.ValidateToken(token);
            if (principal == null)
                return Unauthorized(
                    new ValidateTokenResponseDTO
                    {
                        IsValid = false,
                        Message = "Token inválido o bloqueado",
                    }
                );

            var userId = principal.FindFirst("userId")?.Value;
            var role = principal.FindFirst("role")?.Value;

            return Ok(
                new ValidateTokenResponseDTO
                {
                    IsValid = true,
                    UserId = userId,
                    Role = role,
                }
            );
        }

        /// <summary>
        /// Bloquea el token JWT proporcionado en la cabecera Authorization para cerrar sesión.
        /// </summary>
        /// <returns>Indica que el cierre de sesión fue exitoso.</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { Message = "Token faltante" });

                bool result = _authService.Logout(token);
                if (!result)
                {
                    return BadRequest(new { Message = "Error al cerrar sesión" });
                }
                return Ok(new { Message = "Cierre de sesión exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new { Message = $"Error al procesar la solicitud: {ex.Message}" }
                );
            }
        }
    }
}

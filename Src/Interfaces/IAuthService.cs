using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_auth_service.Src.DTOs;

namespace censudex_auth_service.Src.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de autenticación.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza el proceso de inicio de sesión.
        /// </summary>
        /// <param name="loginRequest">DTO con la información de inicio de sesión.</param>
        /// <returns>DTO con la respuesta del inicio de sesión.</returns>
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequest);
    }
}

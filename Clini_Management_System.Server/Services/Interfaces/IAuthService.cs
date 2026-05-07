using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}

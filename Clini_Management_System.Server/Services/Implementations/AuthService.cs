using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Helpers;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        if (await _userRepository.ClinicExistsAsync(request.ClinicName, ct))
            throw new ConflictException("Clinic already exists.");

        if (await _userRepository.GetByUsernameAsync(request.Username, ct) is not null)
            throw new ConflictException("Username already exists.");

        var clinic = await _userRepository.CreateClinicAsync(new Clinic
        {
            Name = request.ClinicName.Trim()
        }, ct);

        var user = await _userRepository.CreateUserAsync(new User
        {
            Username = request.Username.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = string.IsNullOrWhiteSpace(request.Role) ? "Admin" : request.Role,
            ClinicId = clinic.Id
        }, ct);

        _logger.LogInformation("Registered clinic {ClinicId} with admin {Username}", clinic.Id, user.Username);

        return BuildResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, ct)
                   ?? throw new UnauthorizedException("Invalid credentials.");

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        return BuildResponse(user);
    }

    private AuthResponseDto BuildResponse(User user)
    {
        var (token, expiresAt) = _jwtTokenGenerator.Generate(user);
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = user.Username,
            Role = user.Role,
            ClinicId = user.ClinicId
        };
    }
}

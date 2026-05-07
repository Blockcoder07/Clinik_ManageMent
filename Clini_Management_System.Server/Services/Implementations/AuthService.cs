using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Helpers;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public sealed class AuthService : IAuthService
{
    #region Constants

    private const string DefaultRole = "Admin";

    #endregion

    #region Fields

    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    #endregion

    #region Constructor

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator tokenGenerator,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        await EnsureUniqueAsync(request, ct);

        var clinic = await _userRepository.CreateClinicAsync(new Clinic
        {
            Name = request.ClinicName.Trim()
        }, ct);

        var user = await _userRepository.CreateUserAsync(new User
        {
            Username = request.Username.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = string.IsNullOrWhiteSpace(request.Role) ? DefaultRole : request.Role.Trim(),
            ClinicId = clinic.Id
        }, ct);

        _logger.LogInformation("Registered clinic {ClinicId} with user {Username}", clinic.Id, user.Username);

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

    #endregion

    #region Private Methods

    private async Task EnsureUniqueAsync(RegisterRequestDto request, CancellationToken ct)
    {
        if (await _userRepository.ClinicExistsAsync(request.ClinicName, ct))
            throw new ConflictException("Clinic already exists.");

        if (await _userRepository.GetByUsernameAsync(request.Username, ct) is not null)
            throw new ConflictException("Username already exists.");
    }

    private AuthResponseDto BuildResponse(User user)
    {
        var (token, expiresAt) = _tokenGenerator.Generate(user);
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = user.Username,
            Role = user.Role,
            ClinicId = user.ClinicId
        };
    }

    #endregion
}

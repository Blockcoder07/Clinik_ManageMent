using System.Security.Claims;

namespace Clini_Management_System.Server.Common;

public interface ITenantContext
{
    int ClinicId { get; }
    bool IsAuthenticated { get; }
}

public sealed class TenantContext : ITenantContext
{
    #region Constants

    public const string ClinicIdClaim = "clinicId";

    #endregion

    #region Fields

    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Constructor

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Public Properties

    public int ClinicId
    {
        get
        {
            var raw = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClinicIdClaim);
            return int.TryParse(raw, out var id) ? id : 0;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    #endregion
}

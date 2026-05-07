using System.Security.Claims;

namespace Clini_Management_System.Server.Common;

public interface ITenantContext
{
    int ClinicId { get; }
}

public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int ClinicId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirstValue("clinicId");
            return int.TryParse(value, out var id) ? id : 0;
        }
    }
}

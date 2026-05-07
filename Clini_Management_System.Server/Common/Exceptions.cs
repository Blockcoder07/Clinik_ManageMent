namespace Clini_Management_System.Server.Common;

public class ApiException : Exception
{
    public int StatusCode { get; }

    public ApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class BadRequestException : ApiException
{
    public BadRequestException(string message) : base(message, 400) { }
}

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message) : base(message, 401) { }
}

public class NotFoundException : ApiException
{
    public NotFoundException(string message) : base(message, 404) { }
}

public class ConflictException : ApiException
{
    public ConflictException(string message) : base(message, 409) { }
}

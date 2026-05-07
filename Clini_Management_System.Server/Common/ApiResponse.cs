namespace Clini_Management_System.Server.Common;

public sealed class ApiResponse<T>
{
    #region Properties

    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }

    #endregion

    #region Factory Methods

    public static ApiResponse<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Message = message };

    #endregion
}

namespace Clini_Management_System.Server.Common;

public sealed class PagedResult<T>
{
    #region Constructor

    public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    #endregion

    #region Properties

    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public int TotalPages =>
        PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    #endregion
}

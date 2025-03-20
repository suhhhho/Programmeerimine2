namespace KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;

public abstract class PagedResultBase
{
    [ExcludeFromCodeCoverage]
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }
}
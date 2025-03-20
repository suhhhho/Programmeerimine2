namespace KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;

public class PagedResult<T> : PagedResultBase where T : class
{
    [ExcludeFromCodeCoverage]
    public IList<T> Results { get; set; }

    public PagedResult()
    {
        Results = new List<T>();
    }
}
namespace KooliProjekt.Models;

using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public class InvoiceIndexModel
{
    public InvoiceIndexModel Search { get; set; }
    public PagedResult<Rent> Data { get; set; }

}

namespace KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;

public class Invoice
{
    [ExcludeFromCodeCoverage]
    public int Id { get; set; }
    public int InvoiceNo { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Title { get; set; }
}

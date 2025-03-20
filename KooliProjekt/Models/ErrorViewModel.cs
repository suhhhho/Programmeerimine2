namespace KooliProjekt.Models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
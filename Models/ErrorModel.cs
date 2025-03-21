using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;

namespace TestWebApplication.Models;

/// <summary>
/// Error model data transfer object containing error information specific for endpoints
/// </summary>
public class ErrorModel
{
    private readonly string _genericErrorMessage =
        "An error has occurred while processing your request. Please contact support and mention this error token: {0}";

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorModel"/> class.
    /// </summary>
    /// <param name="ex">The ex.</param>
    /// <param name="errorCode"></param>
    /// <param name="errorDetails"></param>
    public ErrorModel(Exception ex, ErrorCode errorCode, string? errorDetails = null)
    {
        ErrorToken = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture).ToUpperInvariant();
        Message = string.Format(CultureInfo.InvariantCulture, _genericErrorMessage, ErrorToken);
        ErrorDetails = new List<ErrorDetails>
        {
            new() {Details = string.IsNullOrWhiteSpace(errorDetails) ? ex.Message : errorDetails, ErrorCode = errorCode.ToString()}
        };
    }

    /// <summary>
    /// A Guid token to allow the identification of the error inside the API logs (Guid format: B4A2E7D90C5C4E18A466DCAD9625606E)
    /// </summary>
    [Required]
    public string ErrorToken { get; }

    /// <summary>
    /// Consumer friendly message describing the error
    /// </summary>
    [Required]
    public string Message { get; set; }

    /// <summary>
    /// Additional details relevant to the error, used only to enhance debug information
    /// </summary>
    [Required]
    [JsonProperty(PropertyName = "errors")]
    public List<ErrorDetails> ErrorDetails { get; }
}
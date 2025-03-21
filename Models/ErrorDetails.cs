using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestWebApplication.Models;

public class ErrorDetails
{
    /// <summary>
    /// Error code from the following documented list of expected errors.  The description of each response of each endpoint specifies which error codes apply to it. &lt;ul&gt; &lt;li&gt;Validation - The request is not valid. Corresponds with HTTP 400.&lt;/li&gt; &lt;li&gt;UnauthorizedAccess - The request lacks valid authentication credentials. Corresponds with HTTP 401.&lt;/li&gt; &lt;li&gt;NotFound - The resource was not found. Corresponds with HTTP 404.&lt;/li&gt; &lt;li&gt;NoAccess  - The user does not have access to the folder. Corresponds with HTTP 403.&lt;/li&gt; &lt;li&gt;FolderNotFound - the given folder was not found. Corresponds to 409 HTTP Code.&lt;/li&gt; &lt;li&gt;InvalidAuditLogReason - the AuditLog reason is not according with the corresponding folder Audit Trail Policy. Corresponds to 409 HTTP Code.&lt;/li&gt; &lt;li&gt;ConcurrencyError - The item being modified is out of date. The pre-condition check failed. Corresponds with HTTP 412.&lt;/li&gt; &lt;li&gt;InternalServerError - Internal Server Error&lt;li&gt; &lt;/ul&gt;
    /// </summary>
    /// <value>Error code from the following documented list of expected errors.  The description of each response of each endpoint specifies which error codes apply to it. &lt;ul&gt; &lt;li&gt;Validation - The request is not valid. Corresponds with HTTP 400.&lt;/li&gt; &lt;li&gt;UnauthorizedAccess - The request lacks valid authentication credentials. Corresponds with HTTP 401.&lt;/li&gt; &lt;li&gt;NotFound - The resource was not found. Corresponds with HTTP 404.&lt;/li&gt; &lt;li&gt;NoAccess  - The user does not have access to the folder. Corresponds with HTTP 403.&lt;/li&gt; &lt;li&gt;FolderNotFound - the given folder was not found. Corresponds to 409 HTTP Code.&lt;/li&gt; &lt;li&gt;InvalidAuditLogReason - the AuditLog reason is not according with the corresponding folder Audit Trail Policy. Corresponds to 409 HTTP Code.&lt;/li&gt; &lt;li&gt;ConcurrencyError - The item being modified is out of date. The pre-condition check failed. Corresponds with HTTP 412.&lt;/li&gt; &lt;li&gt;InternalServerError - Internal Server Error&lt;li&gt; &lt;/ul&gt;</value>
    [JsonProperty(PropertyName = "errorCode")]
    [Required]
    [JsonConverter(typeof(StringEnumConverter))]
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Human-readable description of the specific error
    /// </summary>
    /// <value>Human-readable description of the specific error</value>

    [JsonProperty(PropertyName = "details")]
    [Required]
    public string? Details { get; set; }
}
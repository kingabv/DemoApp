namespace TestWebApplication.Models;

/// <summary>
/// 
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// Internal Server Error. Corresponds with HTTP 500.
    /// </summary>
    InternalServerError,

    /// <summary>
    /// The request is not valid. Corresponds with HTTP 400.
    /// </summary>
    Validation,

    ///// <summary>
    ///// The request is forbidden. Corresponds with HTTP 403. (in case of authorization)
    ///// </summary>
    //Forbidden,

    /// <summary>
    /// The resource was not found. Corresponds with HTTP 404.
    /// </summary>
    NotFound,

    /// <summary>
    /// The resource could not be processed. Corresponds with HTTP 409.
    /// </summary>
    Conflict,
}
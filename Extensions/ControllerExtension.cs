using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestWebApplication.Models;

namespace TestWebApplication.Extensions;

/// <summary>
/// Controller extension
/// </summary>
public static class ControllerExtension
{
    /// <summary>
    /// 400 - BadRequest
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>	
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static BadRequestObjectResult LogAndReturnBadRequest(this ControllerBase controller, ILogger logger, ErrorModel errorModel)
    {
        logger.LogWarning(errorModel?.ToString());
        return controller.BadRequest(errorModel);
    }

    /// <summary>
    /// 401 - Unauthorized
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>	
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static ObjectResult LogAndReturnUnauthorized(this ControllerBase controller, ILogger logger, ErrorModel errorModel)
    {
        logger.LogWarning(errorModel?.ToString());
        return controller.StatusCode((int)HttpStatusCode.Unauthorized, errorModel);
    }
   
    /// <summary>
    /// 404- NotFound
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>	
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static ObjectResult LogAndReturnNotFound(this ControllerBase controller, ILogger logger, ErrorModel errorModel)
    {
        logger.LogWarning(errorModel?.ToString());
        return controller.NotFound(errorModel);
    }

    /// <summary>
    /// 409 - Conflict
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>	
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static ConflictObjectResult LogAndReturnConflict(this ControllerBase controller, ILogger logger, ErrorModel errorModel)
    {
        logger.LogWarning(errorModel?.ToString());
        return controller.Conflict(errorModel);
    }

    /// <summary>
    /// 500 - InternalServerError
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>	
    /// <param name="exception">The exception threw </param>	
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static ObjectResult LogAndReturnInternalServerError(this ControllerBase controller, ILogger logger,
        Exception exception, ErrorModel errorModel)
    {
        logger.LogError(exception, errorModel?.ToString());
        return controller.StatusCode((int)HttpStatusCode.InternalServerError, errorModel);
    }

    /// <summary>
    /// 415 - UnsupportedMediaType
    /// </summary>
    /// <param name="controller">The controller base </param>    
    /// <param name="logger">The logger </param>
    /// <param name="errorModel">The error model that will be serialized </param>	
    public static ObjectResult LogAndReturnUnsupportedMediaType(this ControllerBase controller, ILogger logger,
        ErrorModel errorModel)
    {
        logger.LogError(errorModel?.ToString());
        return controller.StatusCode((int)HttpStatusCode.UnsupportedMediaType, errorModel);
    }

    /// <summary>
    /// 201 - Created
    /// </summary>
    /// <param name="controller">The controller base </param>	
    /// <param name="logger">The logger </param>
    /// <param name="location">The URI at which the content has been created.</param>
    /// <param name="reportPropertiesToDto">The content value.</param>	
    public static CreatedResult LogAndReturnCreated(this ControllerBase controller, ILogger logger, string location, object reportPropertiesToDto)
    {
        logger.LogInformation("The PDF report was saved successfully"); //NOSONAR
        return controller.Created(new Uri(location, UriKind.Relative), reportPropertiesToDto);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TestWebApplication.Exceptions;
using TestWebApplication.Extensions;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Route("api/Companies")]   
    [ApiController]        
    public class CompanyItemController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly ILogger<CompanyItemController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public CompanyItemController(CompanyContext context, ILogger<CompanyItemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyItem>>> GetCompanyItems()
        {            
            return await _context.CompanyItems.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        // GET: api/Companies/Isin/ala12345612
        [HttpGet("Isin/{isin}")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Request succeeded")]       
        [SwaggerResponse(StatusCodes.Status404NotFound, description: "The specified resource was not found")]        
        public async Task<ActionResult<CompanyItem>> GetCompanyItemByIsin([FromRoute][Required] string isin )
        {
            var companyItem = await _context.CompanyItems.AsQueryable().FirstOrDefaultAsync(p => p.Isin == isin).ConfigureAwait(false);

            if (companyItem == null)
            {
                var ex = new CompanyItemException(ErrorCode.NotFound, string.Join(Environment.NewLine,
                    $"A company with Isin '{isin}' does not exists."));
                return this.LogAndReturnNotFound(_logger, new ErrorModel(ex, ErrorCode.NotFound, ex.Message));
            }

            return companyItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Companies/5
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Request succeeded")]
        [SwaggerResponse(StatusCodes.Status404NotFound, description: "The specified resource was not found")]      
        public async Task<ActionResult<CompanyItem>> GetCompanyItem([FromRoute][Required] int id)
        {
            var companyItem = await _context.CompanyItems.FindAsync(id).ConfigureAwait(false);

            if (companyItem == null)
            {
                var ex = new CompanyItemException(ErrorCode.NotFound, string.Join(Environment.NewLine,
                    $"A company with '{id}' identifier does not exists."));
                return this.LogAndReturnNotFound(_logger, new ErrorModel(ex, ErrorCode.NotFound, ex.Message));
            }

            return companyItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, description: "Request succeeded")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, description: "The specified resource cannot be validated")]
        [SwaggerResponse(StatusCodes.Status409Conflict, description: "The request could not be completed due to a conflict with the current state of the target resource.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, description: "Internal Server Error")]
        public async Task<ActionResult<CompanyItem>> PostCompanyItem([FromBody][Required] CompanyItem company)
        {
            try
            {
                var validationResults = company.ValidateCompanyItemModel().ToList();
                if (validationResults.Any())
                    throw new CompanyItemException(ErrorCode.Validation, string.Join(Environment.NewLine,
                        validationResults.Select(v => v.ErrorMessage)));

                var companyItem = await _context.CompanyItems.FindAsync(company.CompanyID).ConfigureAwait(false);
                if (companyItem != null)
                    throw new CompanyItemException(ErrorCode.Conflict, string.Join(Environment.NewLine,
                        $"A company with '{company.CompanyID}' identifier already exists."));

                company.Isin = company?.Isin?.ToUpperInvariant();
                company.StockTicker = company?.StockTicker?.ToUpperInvariant();

                _context.Add(company);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return CreatedAtAction(nameof(PostCompanyItem), new { id = company?.CompanyID }, company);
            }           
            catch (DbUpdateException ex)
            {
                return this.LogAndReturnBadRequest(_logger,
                            new ErrorModel(ex, ErrorCode.Validation, $"Bad Request:{ex.InnerException?.Message}"));
            }              
            catch (CompanyItemException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCode.Validation:
                        return this.LogAndReturnBadRequest(_logger,
                            new ErrorModel(ex, ErrorCode.Validation, $"Bad Request:{ex.Message}"));
                    case ErrorCode.Conflict:
                        return this.LogAndReturnConflict(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Conflict: {ex.Message}"));                    
                    default:
                        return this.LogAndReturnInternalServerError(_logger, ex,
                            new ErrorModel(ex, ErrorCode.InternalServerError, "Internal Server Error"));
                }
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status201Created, description: "Request succeeded")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, description: "The specified resource cannot be validated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, description: "The specified resource was not found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, description: "The request could not be completed due to a conflict with the current state of the target resource.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, description: "Internal Server Error")]
        public async Task<ActionResult<CompanyItem>> PutCompanyItem(int id, [FromBody][Required] CompanyItem company)
        {
            try
            {
                var validationResults = company.ValidateCompanyItemModel().ToList();
                if (validationResults.Any())
                    throw new CompanyItemException(ErrorCode.Validation, string.Join(Environment.NewLine,
                    validationResults.Select(v => v.ErrorMessage)));

                var companyItem = await _context.CompanyItems.FindAsync(id).ConfigureAwait(false);
                if (companyItem == null)
                    throw new CompanyItemException(ErrorCode.NotFound, string.Join(Environment.NewLine,
                        $"A company with '{id}' identifier does not exists."));               
            
                companyItem.Isin = company?.Isin?.ToUpperInvariant();
                companyItem.StockTicker = company?.StockTicker?.ToUpperInvariant();
                companyItem.Name = company?.Name;
                companyItem.Exchange = company?.Exchange;

                _context.Entry(companyItem).State = EntityState.Modified;
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return CreatedAtAction(nameof(PutCompanyItem), new { id = companyItem?.CompanyID }, companyItem);
            }           
            catch (DbUpdateException ex)
            {
                return this.LogAndReturnConflict(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Conflict:{ex.InnerException?.Message}"));
            }              
            catch (CompanyItemException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCode.Validation:
                        return this.LogAndReturnBadRequest(_logger,
                            new ErrorModel(ex, ErrorCode.Validation, $"Bad Request:{ex.Message}"));
                    case ErrorCode.Conflict:
                        return this.LogAndReturnConflict(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Conflict: {ex.Message}"));
                    case ErrorCode.NotFound:
                        return this.LogAndReturnNotFound(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Not found: {ex.Message}"));                   
                    default:
                        return this.LogAndReturnInternalServerError(_logger, ex,
                            new ErrorModel(ex, ErrorCode.InternalServerError, "Internal Server Error"));
                }
            }                
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApplication.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Index(nameof(Isin), IsUnique = true)]
    public class CompanyItem
    {    
        /// <summary>
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int CompanyID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>Apple Inc</example>
        [Required]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>HEIA</example>
        [Required]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        public string? StockTicker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>NASDAQ</example>
        [Required]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        public string? Exchange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>NL00000000165</example>        
        [Required]
        [StringLength(12, MinimumLength=12, ErrorMessage = "This field must have a length of 12 characters.")]
        public string? Isin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>https://www.apple.com</example>
        [StringLength(30, ErrorMessage = "Max 30 characters")]            
        public string? WebUrl { get; set; }       
    }
}

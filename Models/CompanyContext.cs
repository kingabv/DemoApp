using Microsoft.EntityFrameworkCore;

namespace TestWebApplication.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyContext : DbContext
    {
        private readonly ILogger logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<CompanyItem> CompanyItems { get; set; } = null!;
    }
}

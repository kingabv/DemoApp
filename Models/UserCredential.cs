namespace TestWebApplication.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserCredential
    {
        /// <summary>
        /// 
        /// </summary>
        ///<example>AdminUser</example>
        public required string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///<example>admin</example>
        public required string Password { get; set; }
    }
}

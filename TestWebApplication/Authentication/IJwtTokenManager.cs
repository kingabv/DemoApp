namespace TestWebApplication.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string Authenticate(string username, string password);
    }
}

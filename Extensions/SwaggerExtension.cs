using Swashbuckle.AspNetCore.SwaggerGen;

namespace TestWebApplication.Extensions
{
    internal static class SwaggerExtension
    {
        internal static void AddXmlDocumentationFiles(this SwaggerGenOptions options)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            string[] files = Directory.GetFiles(path, "*.xml");

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    options.IncludeXmlComments(file);
                }
            }
        }
    }
}

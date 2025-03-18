using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace TestWebApplication.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationExtension
    {

        /// <summary>Loads the certificate from store.</summary>
        /// <param name="certificateName">Name of the certificate.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        /// <returns></returns>
        public static X509Certificate2 LoadCertificateFromStore(string certificateName, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            if (certificateName == null)
                throw new ArgumentException(nameof(certificateName));

            using (var store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false).OfType<X509Certificate2>();

                var certificatesWithExactName = certificates.Where(c => c.GetNameInfo(X509NameType.SimpleName, false) == certificateName);

                if (!certificatesWithExactName.Any())
                {
                    var errorMessage =
                        $"No certificate with name '{certificateName}' was found in '{storeLocation}\\{storeName}'";

                    throw new Exception(errorMessage);
                }
                else if (certificatesWithExactName.Count() > 1)
                {
                    var errorMessage =
                        $"More than one certificate with name '{certificateName}' was found in '{storeLocation}\\{storeName}'";
                    throw new Exception(errorMessage);
                }

                return certificatesWithExactName.Single();
            }
        }

    }
}

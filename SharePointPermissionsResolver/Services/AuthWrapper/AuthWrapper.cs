using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SharePointPermissionsResolver.Models;

namespace SharePointPermissionsResolver.Services.AuthWrapper
{
    public class AuthWrapper: IAuthWrapper
    {
        private readonly List<AzureADConfig> azureAdConfig;

        public AuthWrapper(IOptions<List<AzureADConfig>> config)
        {
            this.azureAdConfig = config.Value;
        }

        public async Task<string> GetToken(string spfxPass, string spfxToken, string rootPath = "", bool forGraph = true)
        {
            try
            {
                var config = this.GetAzureADConfigForEnvironment(spfxPass, spfxToken);
                var app = ConfidentialClientApplicationBuilder.Create(config?.ClientId).WithCertificate(this.GetCertificate(config)).Build();
                var scopes = new[] { forGraph ? "https://graph.microsoft.com/.default" : "https://85458q.sharepoint.com/.default" };
                var authResult = await app.AcquireTokenForClient(scopes)
                    .WithAuthority(AzureCloudInstance.AzurePublic, config?.TenantId)
                    .ExecuteAsync();

                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private AzureADConfig? GetAzureADConfigForEnvironment(string spfxPass, string spfxToken)
        {
            var config = this.azureAdConfig.Where(c => c.SpfxPass == spfxPass && c.SpfxToken == spfxToken).FirstOrDefault();

            return config;
        }

        private X509Certificate2 GetCertificate(AzureADConfig? config)
        {
            var cert = new X509Certificate2(
                config?.CertificatePath,
                config?.CertificatePassword,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet
            );

            return cert;
        }
    }
}


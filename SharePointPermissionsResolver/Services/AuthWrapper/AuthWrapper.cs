using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SharePointPermissionsResolver.Models;

namespace SharePointPermissionsResolver.Services.AuthWrapper
{
    public class AuthWrapper: IAuthWrapper
    {
        private readonly AzureADConfig azureAdConfig;

        public AuthWrapper(IOptions<AzureADConfig> config)
        {
            this.azureAdConfig = config.Value;
        }

        public async Task<string> GetToken(string spfxToken, string rootPath = "", bool forGraph = true)
        {
            try
            {
                var environment = this.GetAzureADConfigForEnvironment(spfxToken);
                var app = ConfidentialClientApplicationBuilder.Create(this.azureAdConfig?.ClientId).WithCertificate(this.GetCertificate(this.azureAdConfig)).Build();
                var scopes = new[] { forGraph ? "https://graph.microsoft.com/.default" : $"https://{rootPath}/.default" };
                var authResult = await app.AcquireTokenForClient(scopes)
                    .WithAuthority(AzureCloudInstance.AzurePublic, environment?.TenantId)
                    .ExecuteAsync();

                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private ClientEnvironment? GetAzureADConfigForEnvironment(string spfxToken)
        {
            var environment = this.azureAdConfig.Environments.Where(c => c.SpfxToken == spfxToken).FirstOrDefault();

            return environment;
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


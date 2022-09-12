using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SharePointPermissionsResolver.Models;

namespace SharePointPermissionsResolver.Services.AuthWrapper
{
    public class AuthWrapper: IAuthWrapper
    {
        private IConfidentialClientApplication app;
        private readonly AzureADConfig azureAdConfig;

        public AuthWrapper(IOptions<AzureADConfig> config)
        {
            this.azureAdConfig = config.Value;
            this.app = ConfidentialClientApplicationBuilder.Create(this.azureAdConfig.ClientId).WithClientSecret(this.azureAdConfig.ClientSecret).Build();
        }

        public async Task<string> GetToken()
        {
            var authResult = await this.app.AcquireTokenForClient(scopes: new[] { "https://graph.microsoft.com/.default" })
                   .WithAuthority(AzureCloudInstance.AzurePublic, this.azureAdConfig.TenantId)
                   .ExecuteAsync();

            return authResult.AccessToken;
        }
    }
}


using System;
namespace SharePointPermissionsResolver.Models
{
    public class AzureADConfig
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TenantId { get; set; }
    }
}


using System;
namespace SharePointPermissionsResolver.Models
{
    public class AzureADConfig
    {
        public string ClientId { get; set; }

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }

        public string TenantId { get; set; }

        public string SpfxPass { get; set; }

        public string SpfxToken { get; set; }
    }
}


using System;
namespace SharePointPermissionsResolver.Models
{
    public class AzureADConfig
    {
        public string ClientId { get; set; }

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }

        public List<ClientEnvironment> Environments { get; set; }
    }

    public class ClientEnvironment
    {
        public string TenantId { get; set; }

        public string SpfxToken { get; set; }
    }
}


using System;
namespace SharePointPermissionsResolver.Services.AuthWrapper
{
    public interface IAuthWrapper
    {
        public Task<string> GetToken(string spfxPass, string spfxToken, string rootPath = "", bool forGraph = true);
    }
}


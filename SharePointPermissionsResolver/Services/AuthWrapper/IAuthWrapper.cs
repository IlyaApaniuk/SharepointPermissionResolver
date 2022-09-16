using System;
namespace SharePointPermissionsResolver.Services.AuthWrapper
{
    public interface IAuthWrapper
    {
        public Task<string> GetToken(string spfxToken, string rootPath = "", bool forGraph = true);
    }
}


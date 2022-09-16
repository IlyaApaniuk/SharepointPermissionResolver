using System;
namespace SharePointPermissionsResolver.Models
{
    public class Request
    {
        public string RootPath { get; set; }
        public string ServerRelativePath { get; set; }
        public string ApiUrl { get; set; }
        public string SpfxToken { get; set; }
        public string? DriveName { get; set; }
        public string? Data { get; set; }
    }
}


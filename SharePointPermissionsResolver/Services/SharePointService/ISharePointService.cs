using System;
using SharePointPermissionsResolver.Models;

namespace SharePointPermissionsResolver.Services.SharePointService
{
    public interface ISharePointService
    {
        public Task<string> GetListItems(Request request);

        public Task<bool> CreateListItem(Request request);

        public Task<bool> UpdateListItem(Request request);

        public Task<string> GetDriveItems(Request request);

        public Task<bool> UploadDriveItem(Request request, IFormFile file);
    }
}


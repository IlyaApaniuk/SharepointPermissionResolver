using System;
using SharePointPermissionsResolver.Models;

namespace SharePointPermissionsResolver.Services.SharePointService
{
    public interface ISharePointService
    {
        public Task<ApiResponse> GetListItems(Request request);

        public Task<ApiResponse> CreateListItem(Request request);

        public Task<ApiResponse> UpdateListItem(Request request);

        public Task<ApiResponse> GetDriveItems(Request request);

        public Task<ApiResponse> UploadDriveItem(Request request, IFormFile file);

        public Task<ApiResponse> PerformSearch(Request request);
    }
}


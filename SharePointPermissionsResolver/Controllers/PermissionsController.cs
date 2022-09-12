using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharePointPermissionsResolver.Models;
using SharePointPermissionsResolver.Services.SharePointService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharePointPermissionsResolver.Controllers
{
    [Route("api/[controller]")]
    public class PermissionsController : Controller
    {
        private ISharePointService sharePointService;

        public PermissionsController(ISharePointService service)
        {
            this.sharePointService = service;
        }

        [HttpPost]
        [Route("items/get")]
        public async Task<string> GetListItems([FromBody] Request request)
        {
            var data = await this.sharePointService.GetListItems(request);

            return data;
        }

        [HttpPost]
        [Route("items/create")]
        public async Task<IActionResult> CreateListItem([FromBody] Request request)
        {
            var isCreated = await this.sharePointService.CreateListItem(request);

            return isCreated ? Ok(isCreated) : StatusCode(500);
        }

        [HttpPost]
        [Route("items/update")]
        public async Task<IActionResult> UpdateListItem([FromBody] Request request)
        {
            var isUpdated = await this.sharePointService.UpdateListItem(request);

            return isUpdated ? Ok(isUpdated) : StatusCode(500);
        }

        [HttpPost]
        [Route("drives/get")]
        public async Task<string> GetDriveItems([FromBody] Request request)
        {
            var data = await this.sharePointService.GetDriveItems(request);

            return data;
        }

        [HttpPost]
        [Route("drives/upload")]
        public async Task<IActionResult> UploadDriveItem(IFormFile file, [FromForm] string request)
        {
            var requestData = JsonConvert.DeserializeObject<Request>(request);
            var isUploaded = await this.sharePointService.UploadDriveItem(requestData, file);

            return isUploaded ? Ok(isUploaded) : StatusCode(500);
        }
    }
}


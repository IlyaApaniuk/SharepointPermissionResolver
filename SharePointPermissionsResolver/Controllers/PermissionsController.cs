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
        public async Task<IActionResult> GetListItems([FromBody] Request request)
        {
            try
            {
                var response = await this.sharePointService.GetListItems(request);

                return response.IsSuccessed ? Ok(response.Content) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("items/create")]
        public async Task<IActionResult> CreateListItem([FromBody] Request request)
        {
            try
            {
                var response = await this.sharePointService.CreateListItem(request);

                return response.IsSuccessed ? Ok(true) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("items/update")]
        public async Task<IActionResult> UpdateListItem([FromBody] Request request)
        {
            try
            {
                var response = await this.sharePointService.UpdateListItem(request);

                return response.IsSuccessed ? Ok(true) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("items/search")]
        public async Task<IActionResult> SearchListItems([FromBody] Request request)
        {
            try
            {
                var response = await this.sharePointService.PerformSearch(request);

                return response.IsSuccessed ? Ok(response.Content) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("drives/get")]
        public async Task<IActionResult> GetDriveItems([FromBody] Request request)
        {
            try
            {
                var response = await this.sharePointService.GetDriveItems(request);

                return response.IsSuccessed ? Ok(response.Content) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("drives/upload")]
        public async Task<IActionResult> UploadDriveItem(IFormFile file, [FromForm] string request)
        {
            try
            {
                var requestData = JsonConvert.DeserializeObject<Request>(request);
                var response = await this.sharePointService.UploadDriveItem(requestData, file);

                return response.IsSuccessed ? Ok(true) : BadRequest(response.Content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


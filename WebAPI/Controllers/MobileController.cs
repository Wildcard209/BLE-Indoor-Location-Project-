using DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Shared.Objects;
using System.ComponentModel;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MobileController : ControllerBase
    {
        private readonly ILogger<MobileController> _logger;
        private readonly ContentDataHelper _dataHelper;

        public MobileController(ILogger<MobileController> logger, ContentDataHelper dataHelper)
        {
            _logger = logger;
            _dataHelper = dataHelper;
        }

        [HttpGet]
        [ActionName("MapData")]
        [Description("Gets set up date for the map form the database.")]
        public async Task<IActionResult> GetMapDataAsync()
        { 
            GetPostMapData? mapData = await _dataHelper.GetMapDataAsync();

            if(mapData == null)
            {
                return NotFound("Map data not found.");
            }

            return Ok(mapData);
        }

        [HttpGet]
        [ActionName("Html")]
        [Description("Gets html with the set file name.")]
        public async Task<IActionResult> GetHtmlAsync([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string? content = await _dataHelper.GetHtmlDataAsync(fileName);

            if (string.IsNullOrEmpty(content))
            {
                return NotFound("File not found.");
            }

            return await Task.FromResult(new ContentResult{
                Content = content,
                ContentType = "text/html",
                StatusCode = 200
            });
        }


        [HttpGet]
        [ActionName("JavaScript")]
        [Description("Gets JavaScript file with the set file name.")]
        public async Task<IActionResult> GetJavaScript([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string? content = await _dataHelper.GetJavascriptDataAsync(fileName);

            if (string.IsNullOrEmpty(content))
            {
                return NotFound("File not found.");
            }

            return await Task.FromResult(new ContentResult
            {
                Content = content,
                ContentType = "application/javascript",
                StatusCode = 200
            });
        }

        [HttpGet]
        [ActionName("CSS")]
        [Description("Gets CSS file with the set file name.")]
        public async Task<IActionResult> GetCSS([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string? content = await _dataHelper.GetCssDataAsync(fileName);

            if (string.IsNullOrEmpty(content))
            {
                return NotFound("File not found.");
            }

            return await Task.FromResult(new ContentResult
            {
                Content = content,
                ContentType = "text/css",
                StatusCode = 200
            });
        }

        [HttpGet]
        [ActionName("Image")]
        [Description("Gets image with the set file name.")]
        public IActionResult GetImage([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string filePath = Path.Combine("mobile/image", fileName + ".png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "image/png");
        }

        [HttpGet]
        [ActionName("Map")]
        [Description("Gets image with the set file name.")]
        public IActionResult GetMap()
        {
            string filePath = Path.Combine("mobile/map", "latestMap.png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "image/png");
        }

        [HttpGet]
        [ActionName("Audio")]
        [Description("Gets audio with the set file name.")]
        public IActionResult GetAudio([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string filePath = Path.Combine("mobile/audio", fileName + ".mp3");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "audio/mpeg");
        }

        [HttpGet]
        [ActionName("Video")]
        [Description("Gets video with the set file name.")]
        public IActionResult GetVideo([FromQuery] string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be empty.");
            }

            string filePath = Path.Combine("mobile/video", fileName + ".mp4");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "video/mp4");
        }
    }
}


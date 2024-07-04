using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Hosting.Internal;
using Org.BouncyCastle.Asn1.X509;
using Shared.Objects;
using System.ComponentModel;
using System.IO;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    // Turned off authorizeation for now
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class WebController : Controller
    {
        private readonly ILogger<WebController> _logger;
        private readonly ContentDataHelper _dataHelper;

        public WebController(ILogger<WebController> logger, ContentDataHelper dataHelper)
        {
            _logger = logger;
            _dataHelper = dataHelper;
        }
       
        [HttpGet]
        [ActionName("AvailableFiles")]
        [Description("Gets all files that are available inside of the web api so far")]
        public async Task<IActionResult> GetAvailableFilesAsync()
        {

            string baseFolder = Path.Combine("mobile/");
            List<string> folders = new() { "image", "video", "audio", "map" };
            List<Tuple<string?, string?>> fileList = new();

            foreach (string folder in folders)
            {
                string path = Path.Combine(baseFolder, folder);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string[] fileNameSplit = fileName.Split('.');
                    fileList.Add(new Tuple<string?, string?>(folder, fileNameSplit[0]));
                }
            }

            foreach (string htmlName in await _dataHelper.GetAllHtmlNamesAsync())
            {
                fileList.Add(new Tuple<string?, string?>("html", htmlName));
            }

            foreach (string jsName in await _dataHelper.GetAllJavascriptNamesAsync())
            {
                fileList.Add(new Tuple<string?, string?>("js", jsName));
            }

            foreach (string cssName in await _dataHelper.GetAllCssNamesAsync())
            {
                fileList.Add(new Tuple<string?, string?>("css", cssName));
            }

            foreach (string popupName in await _dataHelper.GetAllPopupNamesAsync())
            {
                fileList.Add(new Tuple<string?, string?>("popup", popupName));
            }

            return await Task.FromResult( Ok(fileList));
        }

        [HttpPatch]
        [ActionName("Map")]
        [Description("Updates the current map for the project and places the old one inside the images folder")]
        public async Task<IActionResult> UpdateMapAsync([FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".png")
            {
                return BadRequest("Only PNG files are accepted.");
            }

            var imagePath = Path.Combine("mobile/", "image");
            var mapPath = Path.Combine("mobile/", "map");
            string oldMapFileName = $"oldMap_{DateTime.UtcNow:yyyyMMddHHmmss}.png";
            string oldMapPath = Path.Combine(imagePath, oldMapFileName);
            string latestMapPath = Path.Combine(mapPath, "latestMap.png");

            try
            {
                if (!Directory.Exists(mapPath))
                {
                    Directory.CreateDirectory(mapPath);
                }

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                if (System.IO.File.Exists(latestMapPath) && isFirstChunk)
                {
                    System.IO.File.Move(latestMapPath, oldMapPath);
                }

                FileMode fileMode = isFirstChunk ? FileMode.Create : FileMode.Append;

                using (var stream = new FileStream(latestMapPath, fileMode))
                using (var bufferedStream = new BufferedStream(stream))
                {
                    await file.CopyToAsync(bufferedStream);
                }

                return Ok("Map updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [ActionName("Image")]
        [Description("Uploads a new image to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateImageAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".png")
            {
                return BadRequest("Only PNG files are accepted.");
            }
            
            var imageDirectory = Path.Combine("mobile/", "image");
            string imageFileName = $"{fileName}.png";
            string imagePath = Path.Combine(imageDirectory, imageFileName);

            try
            {
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                if (System.IO.File.Exists(imagePath) && isFirstChunk)
                {
                    System.IO.File.Delete(imagePath);
                }

                FileMode fileMode = isFirstChunk ? FileMode.Create : FileMode.Append;

                using (var stream = new FileStream(imagePath, fileMode))
                using (var bufferedStream = new BufferedStream(stream))
                {
                    await file.CopyToAsync(bufferedStream);
                }

                return Ok("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Image")]
        [Description("Removes all files that match the list of file names given form images.")]
        public async Task<IActionResult> DeleteImageAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            var imageDirectory = Path.Combine("mobile/", "image");
            try
            {
                foreach (string? fileName in fileNames)
                {
                    string imageFileName = $"{fileName}.png";
                    string imagePath = Path.Combine(imageDirectory, imageFileName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                return await Task.FromResult(Ok("Image removed successfully."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(500, $"An error occurred: {ex.Message}"));
            }
        }


        [HttpPost]
        [ActionName("Html")]
        [Description("Uploads a new html file to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateHtmlAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".html")
            {
                return BadRequest("Only HTML files are accepted.");
            }

            try
            {
                string fileContent = "";
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    fileContent = System.Text.Encoding.UTF8.GetString(bytes);
                }

                if(string.IsNullOrEmpty(fileContent))
                {
                    throw new Exception("No data in file");
                }

                if (isFirstChunk)
                {
                    await _dataHelper.AddHtmlDataAsync(fileName, fileContent);
                }
                else
                {
                    await _dataHelper.AppendHtmlDataAsync(fileName, fileContent);
                }

                return Ok("Html uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Html")]
        [Description("Removes all files that match the list of file names given form html.")]
        public async Task<IActionResult> DeleteHtmlAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            await _dataHelper.RemoveHtmlAsync(fileNames);

            return await Task.FromResult(Ok("Html removed successfully."));
        }

        [HttpPost]
        [ActionName("Css")]
        [Description("Uploads a new css file to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateCssAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".css")
            {
                return BadRequest("Only CSS files are accepted.");
            }

            try
            {
                string fileContent = "";
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    fileContent = System.Text.Encoding.UTF8.GetString(bytes);

                }

                if (string.IsNullOrEmpty(fileContent))
                {
                    throw new Exception("No data in file");
                }

                if (isFirstChunk)
                {
                    await _dataHelper.AddCssDataAsync(fileName, fileContent);
                }
                else
                {
                    await _dataHelper.AppendCssDataAsync(fileName, fileContent);
                }

                return Ok("Css uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Css")]
        [Description("Removes all files that match the list of file names given form css.")]
        public async Task<IActionResult> DeleteCssAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            await _dataHelper.RemoveCssAsync(fileNames);

            return await Task.FromResult(Ok("CSS removed successfully."));
        }

        [HttpPost]
        [ActionName("Javascript")]
        [Description("Uploads a new javascript file to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateJavascriptAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".js")
            {
                return BadRequest("Only Javascript files are accepted.");
            }

            try
            {
                string fileContent = "";
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    fileContent = System.Text.Encoding.UTF8.GetString(bytes);

                }

                if (string.IsNullOrEmpty(fileContent))
                {
                    throw new Exception("No data in file");
                }

                if (isFirstChunk)
                {
                    await _dataHelper.AddJavascriptDataAsync(fileName, fileContent);
                }
                else
                {
                    await _dataHelper.AppendJavascriptDataAsync(fileName, fileContent);
                }

                return Ok("Javascript uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Javascript")]
        [Description("Removes all files that match the list of file names given form Javascript.")]
        public async Task<IActionResult> DeleteJavascriptAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            await _dataHelper.RemoveJavascriptlAsync(fileNames);

            return await Task.FromResult(Ok("Javascript removed successfully."));
        }


        [HttpPost]
        [ActionName("Video")]
        [Description("Uploads a new video to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateVideoAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".mp4") { 
                return BadRequest("Only MP4 files are accepted.");
            }

            var htmlDirectory = Path.Combine("mobile/", "video");
            string imageFileName = $"{fileName}.mp4";
            string imagePath = Path.Combine(htmlDirectory, imageFileName);

            try
            {
                if (!Directory.Exists(htmlDirectory))
                {
                    Directory.CreateDirectory(htmlDirectory);
                }

                if (System.IO.File.Exists(imagePath) && isFirstChunk)
                {
                    System.IO.File.Delete(imagePath);
                }

                FileMode fileMode = isFirstChunk ? FileMode.Create : FileMode.Append;

                using (var stream = new FileStream(imagePath, fileMode))
                using (var bufferedStream = new BufferedStream(stream))
                {
                    await file.CopyToAsync(bufferedStream);
                }

                return Ok("Video uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Video")]
        [Description("Removes all files that match the list of file names given form videos.")]
        public async Task<IActionResult> DeleteVideoAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            var imageDirectory = Path.Combine("mobile/", "video");
            try
            {
                foreach (string? fileName in fileNames)
                {
                    string imageFileName = $"{fileName}.mp4";
                    string imagePath = Path.Combine(imageDirectory, imageFileName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                return await Task.FromResult(Ok("Video removed successfully."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(500, $"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        [ActionName("Audio")]
        [Description("Uploads a new audio to the web api, if the file name is the same as a file on the system, it then replaces it.")]
        public async Task<IActionResult> UpdateAudioAsync([FromQuery] string? fileName, [FromQuery] bool isFirstChunk, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("No file name provided.");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".mp3")
            {
                return BadRequest("Only MP4 files are accepted.");
            }

            var htmlDirectory = Path.Combine("mobile/", "audio");
            string imageFileName = $"{fileName}.mp3";
            string imagePath = Path.Combine(htmlDirectory, imageFileName);
            try
            {
                if (!Directory.Exists(htmlDirectory))
                {
                    Directory.CreateDirectory(htmlDirectory);
                }

                if (System.IO.File.Exists(imagePath) && isFirstChunk)
                {
                    System.IO.File.Delete(imagePath);
                }

                FileMode fileMode = isFirstChunk ? FileMode.Create : FileMode.Append;

                using (var stream = new FileStream(imagePath, fileMode))
                using (var bufferedStream = new BufferedStream(stream))
                {
                    await file.CopyToAsync(bufferedStream);
                }

                return Ok("Audio uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [ActionName("Audio")]
        [Description("Removes all files that match the list of file names given form audios.")]
        public async Task<IActionResult> DeleteAudioAsync([FromQuery] List<string?>? fileNames)
        {
            if (fileNames == null)
            {
                return await Task.FromResult(BadRequest("No files name provided."));
            }

            var imageDirectory = Path.Combine("mobile/", "audio");
            try
            {
                foreach (string? fileName in fileNames)
                {
                    string imageFileName = $"{fileName}.mp3";
                    string imagePath = Path.Combine(imageDirectory, imageFileName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                return await Task.FromResult(Ok("Audio removed successfully."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(500, $"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet]
        [ActionName("Popup")]
        [Description("Gets the html content assosiated with a popup name")]
        public async Task<IActionResult> GetPopupAstnc([FromQuery] string? fileName)
        {
            if (fileName == null)
            {
                return await Task.FromResult(BadRequest("No file name provided."));
            }

            string? popupData = await _dataHelper.GetPopupDataAsync(fileName);

            if(popupData == null)
            {
                return await Task.FromResult(BadRequest("No file found."));
            }

            return await Task.FromResult(Ok(popupData));
        }

        [HttpPost]
        [ActionName("Popup")]
        [Description("Adds or updates a popup data with html content.")]
        public async Task<IActionResult> AddPopupAsync([FromQuery] string? fileName, [FromQuery] string? content)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                return await Task.FromResult(BadRequest("No file name provided."));
            }
            if (string.IsNullOrEmpty(content))
            {
                return await Task.FromResult(BadRequest("No content provided."));
            }

            await _dataHelper.AddPopupDataAsync(fileName, content);

            return await Task.FromResult(Ok());
        }

        [HttpPatch]
        [ActionName("MainData")]
        [Description("Updates the map data used for the mobile app, if their is no data, then it creates a new way to store data instead")]
        public async Task<IActionResult> UpdateMainDataAsync([FromBody] GetPostMapData? mapData)
        {
            if (mapData == null)
            {
                return await Task.FromResult(BadRequest("No file name provided."));
            }
            await _dataHelper.UpdateMapDataAsync(mapData);

            return await Task.FromResult(Ok());
        }

        private void UnlockFile(string filePath)
        {
            FileStream stream = null;
            try
            {
                stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                stream.Close();
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}

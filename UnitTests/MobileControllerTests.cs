using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
namespace UnitTests
{
    [TestClass]
    public class MobileControllerTests
    {
        private MobileController _mobileController;
        private Mock<ILogger<MobileController>> _loggerMock;
        private ContentDataHelper _dataHelper;

        [TestInitialize]
        public void Setup()
        {
            //Server should be to a local mysql database, please do not use a live db for unit tests
            string connectionString = "";

            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                .UseMySQL(connectionString)
                .Options;

            Context dbContext = new(options);

            _loggerMock = new Mock<ILogger<MobileController>>();
            _dataHelper = new ContentDataHelper(dbContext);
            _mobileController = new MobileController(_loggerMock.Object, _dataHelper);
        }

        [TestMethod]
        public async Task GetMapDataAsync_MapDataNotFound_ReturnsNotFound_0r_MapDataFound_ReturnsOk()
        {
            IActionResult result = await _mobileController.GetMapDataAsync();

            Assert.IsNotNull(result);
            if (result is NotFoundObjectResult notFoundResult)
            {
                Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.AreEqual("Map data not found.", notFoundResult.Value);
            }
            else if (result is OkObjectResult okResult)
            {
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            }
            else
            {
                Assert.Fail("Unexpected result type.");
            }
        }

        [TestMethod]
        public async Task GetHtmlAsync_EmptyFileName_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _mobileController.GetHtmlAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("File name cannot be empty.", result.Value);
        }

        [TestMethod]
        public async Task GetHtmlAsync_HtmlContentNotFound_ReturnsNotFound()
        {
            string fileName = "nonexistent";

            NotFoundObjectResult? result = await _mobileController.GetHtmlAsync(fileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);
        }

        [TestMethod]
        public async Task GetHtmlAsync_ValidFileName_ReturnsContentResult()
        {
            string fileName = "test";
            string htmlContent = "<html><body><h1>Hello, world!</h1></body></html>";

            await _dataHelper.AddHtmlDataAsync(fileName, htmlContent);

            ContentResult? result = await _mobileController.GetHtmlAsync(fileName) as ContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(htmlContent, result.Content);
            Assert.AreEqual("text/html", result.ContentType);
        }

        [TestMethod]
        public async Task GetJavaScript_EmptyFileName_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _mobileController.GetJavaScript(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("File name cannot be empty.", result.Value);
        }

        [TestMethod]
        public async Task GetJavaScript_JavaScriptContentNotFound_ReturnsNotFound()
        {
            string fileName = "nonexistent";

            NotFoundObjectResult? result = await _mobileController.GetJavaScript(fileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);
        }

        [TestMethod]
        public async Task GetJavaScript_ValidFileName_ReturnsContentResult()
        {
            string fileName = "test";
            string jsContent = "console.log('Hello, world!');";

            await _dataHelper.AddJavascriptDataAsync(fileName, jsContent);

            ContentResult? result = await _mobileController.GetJavaScript(fileName) as ContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(jsContent, result.Content);
            Assert.AreEqual("application/javascript", result.ContentType);
        }

        [TestMethod]
        public async Task GetCSS_EmptyFileName_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _mobileController.GetCSS(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("File name cannot be empty.", result.Value);
        }

        [TestMethod]
        public async Task GetCSS_CssContentNotFound_ReturnsNotFound()
        {
            string fileName = "nonexistent";

            NotFoundObjectResult? result = await _mobileController.GetCSS(fileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);
        }

        [TestMethod]
        public async Task GetCSS_ValidFileName_ReturnsContentResult()
        {
            string fileName = "test";
            string cssContent = "body { background-color: red; }";

            await _dataHelper.AddCssDataAsync(fileName, cssContent);

            ContentResult? result = await _mobileController.GetCSS(fileName) as ContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(cssContent, result.Content);
            Assert.AreEqual("text/css", result.ContentType);
        }

        [TestMethod]
        public void GetImage_EmptyFileName_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = _mobileController.GetImage(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("File name cannot be empty.", result.Value);
        }

        [TestMethod]
        public void GetImage_ImageNotFound_ReturnsNotFound()
        {
            string fileName = "nonexistent";

            NotFoundObjectResult? result = _mobileController.GetImage(fileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);
        }


        [TestMethod]
        public void GetImage_ValidFileName_ReturnsFileStreamResult()
        {
            string fileName = "testImage";
            string expectedFilePath = Path.Combine("mobile/image", fileName + ".png");

            using (Bitmap bitmap = new Bitmap(100, 100))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                }
                bitmap.Save(expectedFilePath, ImageFormat.Png);
            }

            long expectedFileStreamLength = new FileInfo(expectedFilePath).Length;

            FileStreamResult? result = _mobileController.GetImage(fileName) as FileStreamResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("image/png", result.ContentType);
            Assert.IsNotNull(result.FileStream);
            Assert.AreEqual(expectedFileStreamLength, result.FileStream.Length);
        }

        [TestMethod]
        public void GetMap_FileDoesNotExist_ReturnsNotFound()
        {
            string filePath = Path.Combine("mobile/map", "latestMap.png");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            NotFoundObjectResult? result = _mobileController.GetMap() as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);

            //recreat map just for other tests
            using (Bitmap bitmap = new Bitmap(100, 100))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                }
                bitmap.Save(Path.Combine("mobile/map", "latestMap.png"), ImageFormat.Png);
            }
        }

        [TestMethod]
        public void GetMap_ValidFileName_ReturnsFileStreamResult()
        {
            string expectedFilePath = Path.Combine("mobile/map", "latestMap.png");

            if (!File.Exists(expectedFilePath))
            {
                using (Bitmap bitmap = new Bitmap(100, 100))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.White);
                    }
                    bitmap.Save(expectedFilePath, ImageFormat.Png);
                }
            }

            long expectedFileStreamLength = new FileInfo(expectedFilePath).Length;

            FileStreamResult? result = _mobileController.GetMap() as FileStreamResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("image/png", result.ContentType);
            Assert.IsNotNull(result.FileStream);
            Assert.AreEqual(expectedFileStreamLength, result.FileStream.Length);
        }

        [TestMethod]
        public void GetAudio_FileDoesNotExist_ReturnsNotFound()
        {
            string fileName = "nonexistent";

            NotFoundObjectResult? result = _mobileController.GetAudio(fileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("File not found.", result.Value);
        }

        [TestMethod]
        public void GetAudio_FileExists_ReturnsFileStreamResult()
        {
            string fileName = "testAudio";
            string expectedFilePath = Path.Combine("mobile/audio", fileName + ".mp3");

            if (!File.Exists(expectedFilePath))
            {
                if (!File.Exists(expectedFilePath))
                {
                    using (FileStream audioFileStream = File.Create(expectedFilePath))
                    {
                        byte[] dummyAudioData = new byte[1024];
                        new Random().NextBytes(dummyAudioData);
                        audioFileStream.Write(dummyAudioData, 0, dummyAudioData.Length);
                    }
                }
            }

            long expectedFileStreamLength = new FileInfo(expectedFilePath).Length;

            FileStreamResult? result = _mobileController.GetAudio(fileName) as FileStreamResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("audio/mpeg", result.ContentType);
            Assert.IsNotNull(result.FileStream);
            Assert.AreEqual(expectedFileStreamLength, result.FileStream.Length);
        }

        [TestMethod]
        public void GetVideo_ReturnsFile_FileStreamResult()
        {
            string fileName = "testVideo";
            string expectedFilePath = Path.Combine("mobile/video", fileName + ".mp4");

            if (!File.Exists(expectedFilePath))
            {
                using (FileStream videoFileStream = File.Create(expectedFilePath))
                {
                    byte[] dummyVideoData = new byte[1024]; 
                    new Random().NextBytes(dummyVideoData); 
                    videoFileStream.Write(dummyVideoData, 0, dummyVideoData.Length);
                }
            }

            FileStreamResult? result = _mobileController.GetVideo(fileName) as FileStreamResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("video/mp4", result.ContentType);
            Assert.IsNotNull(result.FileStream);
        }

        [TestMethod]
        public void GetVideo_FileNotFound_ReturnsNotFound()
        {
            string nonExistentFileName = "nonexistent";

            NotFoundObjectResult? result =  _mobileController.GetVideo(nonExistentFileName) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("File not found.", result.Value);
        }

        [TestMethod]
        public void GetVideo_InvalidFileName_ReturnsBadRequest()
        {
            string invalidFileName = null;

            BadRequestObjectResult? result = _mobileController.GetVideo(invalidFileName) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("File name cannot be empty.", result.Value);
        }
    }
}

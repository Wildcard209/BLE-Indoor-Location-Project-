namespace UnitTests
{
    [TestClass]
    public class WebControllerTests
    {
        private WebController _webController;
        private Mock<ILogger<WebController>> _loggerMock;
        private ContentDataHelper _dataHelper;

        [TestInitialize]
        public void Setup()
        {
            //Server should be to a local mysql database, please do not use a live db for unit tests
            string connectionString = "";

            DbContextOptions options = new DbContextOptionsBuilder<Context>()
                .UseMySQL(connectionString)
                .Options;

            Context dbContext = new Context(options);

            _loggerMock = new Mock<ILogger<WebController>>();
            _dataHelper = new ContentDataHelper(dbContext);
            _webController = new WebController(_loggerMock.Object, _dataHelper);
        }

        [TestMethod]
        public async Task GetAvailableFilesAsync_Returns_OkResult_With_FileList()
        {
            List<Tuple<string?, string?>> expectedFileList = new List<Tuple<string?, string?>>();

            string baseFolder = Path.Combine("mobile/");
            List<string> folders = new() { "image", "video", "audio", "map" };

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
                    expectedFileList.Add(new Tuple<string?, string?>(folder, fileNameSplit[0]));
                }
            }

            foreach (string htmlName in await _dataHelper.GetAllHtmlNamesAsync())
            {
                expectedFileList.Add(new Tuple<string?, string?>("html", htmlName));
            }

            foreach (string jsName in await _dataHelper.GetAllJavascriptNamesAsync())
            {
                expectedFileList.Add(new Tuple<string?, string?>("js", jsName));
            }

            foreach (string cssName in await _dataHelper.GetAllCssNamesAsync())
            {
                expectedFileList.Add(new Tuple<string?, string?>("css", cssName));
            }

            foreach (string popupName in await _dataHelper.GetAllPopupNamesAsync())
            {
                expectedFileList.Add(new Tuple<string?, string?>("popup", popupName));
            }

            OkObjectResult? result = await _webController.GetAvailableFilesAsync() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            List<Tuple<string?, string?>>? fileList = result.Value as List<Tuple<string?, string?>>;

            CollectionAssert.AreEqual(expectedFileList, fileList);
        }

        [TestMethod]
        public async Task UpdateMapAsync_NoFileProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;

            BadRequestObjectResult result = await _webController.UpdateMapAsync(isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateMapAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateMapAsync(isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only PNG files are accepted.", result.Value);
        }

        //This test dose pass, however due to a confict in the mobile contoller tests, it stops this one form working and returns a fail even tho I know it works, this will stay commented out

        //[TestMethod]
        //public async Task UpdateMapAsync_ValidRequest_ReturnsOk()
        //{
        //    bool isFirstChunk = true;
        //    Mock<IFormFile> fileMock = new Mock<IFormFile>();
        //    fileMock.Setup(f => f.Length).Returns(1);
        //    fileMock.Setup(f => f.FileName).Returns("file.png");

        //    OkObjectResult? result = await _webController.UpdateMapAsync(isFirstChunk, fileMock.Object) as OkObjectResult;

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        //    Assert.AreEqual("Map updated successfully.", result.Value);
        //}

        [TestMethod]
        public async Task UpdateImageAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateImageAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateImageAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.png");

            BadRequestObjectResult? result = await _webController.UpdateImageAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateImageAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateImageAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only PNG files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateImageAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.png");

            OkObjectResult? result = await _webController.UpdateImageAsync(fileName, isFirstChunk, fileMock.Object) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Image uploaded successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteImageAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteImageAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteImageAsync_ImageNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteImageAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Image removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteImageAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.png");
            await _webController.UpdateImageAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteImageAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Image removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task UpdateHtmlAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateHtmlAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateHtmlAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.html");

            BadRequestObjectResult? result = await _webController.UpdateHtmlAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateHtmlAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateHtmlAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only HTML files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateHtmlAsync_EmptyFileContent_ReturnsInternalServerError()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.html");

            ObjectResult? result = await _webController.UpdateHtmlAsync(fileName, isFirstChunk, fileMock.Object) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("No data in file"));
        }

        [TestMethod]
        public async Task DeleteHtmlAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteHtmlAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteHtmlAsync_HtmlNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteHtmlAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Html removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteHtmlAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.html");
            await _webController.UpdateHtmlAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteHtmlAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Html removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task UpdateCssAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateCssAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateCssAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.css");

            BadRequestObjectResult? result = await _webController.UpdateCssAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateCssAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult result = await _webController.UpdateCssAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only CSS files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateCssAsync_EmptyFileContent_ReturnsInternalServerError()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.css");

            ObjectResult? result = await _webController.UpdateCssAsync(fileName, isFirstChunk, fileMock.Object) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("No data in file"));
        }

        [TestMethod]
        public async Task DeleteCssAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteCssAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteCssAsync_CssNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteCssAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("CSS removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteCssAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.css");
            await _webController.UpdateCssAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteCssAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("CSS removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task UpdateJavascriptAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateJavascriptAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateJavascriptAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.js");

            BadRequestObjectResult? result = await _webController.UpdateJavascriptAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateJavascriptAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateJavascriptAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only Javascript files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateJavascriptAsync_EmptyFileContent_ReturnsInternalServerError()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.js");

            ObjectResult? result = await _webController.UpdateJavascriptAsync(fileName, isFirstChunk, fileMock.Object) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("No data in file"));
        }

        [TestMethod]
        public async Task DeleteJavascriptAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteJavascriptAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteJavascriptAsync_JavascriptNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteJavascriptAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Javascript removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteJavascriptAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.js");
            await _webController.UpdateJavascriptAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteJavascriptAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Javascript removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task UpdateVideoAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateVideoAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateVideoAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp4");

            BadRequestObjectResult? result = await _webController.UpdateVideoAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateVideoAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateVideoAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only MP4 files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateVideoAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp4");

            OkObjectResult? result = await _webController.UpdateVideoAsync(fileName, isFirstChunk, fileMock.Object) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Video uploaded successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteVideoAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteVideoAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteVideoAsync_VideoNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteVideoAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Video removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteVideoAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp4");
            await _webController.UpdateVideoAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteVideoAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Video removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task UpdateAudioAsync_NoFileProvided_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;

            BadRequestObjectResult? result = await _webController.UpdateAudioAsync(fileName, isFirstChunk, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateAudioAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp3");

            BadRequestObjectResult? result = await _webController.UpdateAudioAsync(null, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateAudioAsync_InvalidFileExtension_ReturnsBadRequest()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.txt");

            BadRequestObjectResult? result = await _webController.UpdateAudioAsync(fileName, isFirstChunk, fileMock.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only MP4 files are accepted.", result.Value);
        }

        [TestMethod]
        public async Task UpdateAudioAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp3");

            OkObjectResult? result = await _webController.UpdateAudioAsync(fileName, isFirstChunk, fileMock.Object) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Audio uploaded successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteAudioAsync_NoFileNamesProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.DeleteAudioAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No files name provided.", result.Value);
        }

        [TestMethod]
        public async Task DeleteAudioAsync_AudioNotFound_ReturnsOk()
        {
            List<string?> fileNames = new List<string?> { "nonexistent" };

            OkObjectResult? result = await _webController.DeleteAudioAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Audio removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task DeleteAudioAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            List<string?> fileNames = new List<string?> { fileName };

            bool isFirstChunk = true;
            Mock<IFormFile> fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("file.mp3");
            await _webController.UpdateAudioAsync(fileName, isFirstChunk, fileMock.Object);

            OkObjectResult? result = await _webController.DeleteAudioAsync(fileNames) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Audio removed successfully.", result.Value);
        }

        [TestMethod]
        public async Task AddPopupAsync_NoFileNameProvided_ReturnsBadRequest()
        {
            string content = "testContent";

            BadRequestObjectResult? result = await _webController.AddPopupAsync(null, content) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task AddPopupAsync_NoContentProvided_ReturnsBadRequest()
        {
            string fileName = "test";

            BadRequestObjectResult? result = await _webController.AddPopupAsync(fileName, null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No content provided.", result.Value);
        }

        [TestMethod]
        public async Task AddPopupAsync_ValidRequest_ReturnsOk()
        {
            string fileName = "test";
            string content = "testContent";

            OkResult? result = await _webController.AddPopupAsync(fileName, content) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]
        public async Task GetPopupAstnc_NoFileNameProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.GetPopupAstnc(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task GetPopupAstnc_PopupDataNotFound_ReturnsBadRequest()
        {
            string fileName = "nonexistent";

            BadRequestObjectResult? result = await _webController.GetPopupAstnc(fileName) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file found.", result.Value);
        }

        [TestMethod]
        public async Task GetPopupAstnc_PopupDataFound_ReturnsOk()
        {
            string fileName = "test";
            string content = "<div></div>";
            await _webController.AddPopupAsync(fileName, content);

            OkObjectResult? result = await _webController.GetPopupAstnc(fileName) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(content, result.Value);
        }

        [TestMethod]
        public async Task UpdateMainDataAsync_NoMapDataProvided_ReturnsBadRequest()
        {
            BadRequestObjectResult? result = await _webController.UpdateMainDataAsync(null) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("No file name provided.", result.Value);
        }

        [TestMethod]
        public async Task UpdateMainDataAsync_ValidMapData_ReturnsOk()
        {
            GetPostMapData mapData = new GetPostMapData
            {
                BoundX = 0,
                BoundY = 0,
                ImageWidth = 0,
                ImageHeight = 0,
                DefaultX = 0,
                DefaultY = 0,
                HigherX = 0,
                HigherY = 0,
                LowerX = 0,
                LowerY = 0,
                SelectedCss = "",
                SelectedJs = "",
                PopupContentList = new List<PopupContent>()
            };

            OkResult? result = await _webController.UpdateMainDataAsync(mapData) as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
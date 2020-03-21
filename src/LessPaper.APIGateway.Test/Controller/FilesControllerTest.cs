using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LessPaper.APIGateway.Controllers.v1;
using LessPaper.APIGateway.Models.Request;
using LessPaper.APIGateway.Models.Response;
using LessPaper.Shared.Enums;
using LessPaper.Shared.Interfaces.GuardApi;
using LessPaper.Shared.Interfaces.ReadApi;
using LessPaper.Shared.Interfaces.ReadApi.ObjectApi;
using LessPaper.Shared.Interfaces.WriteApi;
using LessPaper.Shared.Interfaces.WriteApi.ObjectApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LessPaper.APIGateway.UnitTest.Controller
{
    public class FilesControllerTest : BaseController
    {
        private readonly byte[] myFile;
        private readonly UploadFileRequest request;
        private readonly MemoryStream stream;
        public FilesControllerTest()
        {
            // Setup dummy request
            myFile = new byte[] { 0, 1, 2, 3 };
            stream = new MemoryStream(myFile);
            IFormFile file = new FormFile(stream, 0, myFile.Length, "name", "fileName");
            request = new UploadFileRequest()
            {
                PlaintextKey = "MyPlaintextKey",
                EncryptedKey = "MyEncryptedKey",
                File = file,
                Name = "MyDoc.pdf",
                DocumentLanguage = "DE"
            };
        }

        [Fact]
        public async void UploadFileToUnknownLocation_Ok()
        {
            // Setup dummy response
            var uploadResponse = new Mock<IUploadMetadata>();
            uploadResponse.SetupGet(x => x.ObjectId).Returns("MyId");
            uploadResponse.SetupGet(x => x.SizeInByte).Returns((uint)myFile.Length);
            uploadResponse.SetupGet(x => x.QuickNumber).Returns(1);
            uploadResponse.SetupGet(x => x.ObjectName).Returns(request.Name);

            // Mock apis
            var readApiMock = new Mock<IReadApi>();
            var writeApiMock = new Mock<IWriteApi>();
            writeApiMock.Setup(mock => 
                mock.ObjectApi.UploadFile(It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
            ).ReturnsAsync(uploadResponse.Object);

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            // Query controller
            var response = await controller.UploadFileToUnknownLocation(request);
            var metadataResponseObject = Assert.IsType<OkObjectResult>(response);
            var metadataResponse = Assert.IsType<UploadFileResponse>(metadataResponseObject.Value);

            // Compare values
            Assert.Equal(uploadResponse.Object.ObjectId, metadataResponse.ObjectId);
            Assert.Equal(uploadResponse.Object.QuickNumber, metadataResponse.QuickNumber);
            Assert.Equal(uploadResponse.Object.ObjectName, metadataResponse.ObjectName);
            Assert.Equal(uploadResponse.Object.SizeInByte, metadataResponse.SizeInByte);
        }
        
        [Fact]
        public async void UploadFileToKnownLocation_Ok()
        {
            // Setup dummy response
            var uploadResponse = new Mock<IUploadMetadata>();
            uploadResponse.SetupGet(x => x.ObjectId).Returns("MyId");
            uploadResponse.SetupGet(x => x.SizeInByte).Returns((uint)myFile.Length);
            uploadResponse.SetupGet(x => x.QuickNumber).Returns(1);
            uploadResponse.SetupGet(x => x.ObjectName).Returns(request.Name);

            // Mock apis
            var readApiMock = new Mock<IReadApi>();
            var writeApiMock = new Mock<IWriteApi>();
            writeApiMock.Setup(mock =>
                mock.ObjectApi.UploadFile(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
            ).ReturnsAsync(uploadResponse.Object);

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);
            
            // Query controller
            var response = await controller.UploadFileToKnownLocation(request, "myDirectoryId", 4);
            var metadataResponseObject = Assert.IsType<OkObjectResult>(response);
            var metadataResponse = Assert.IsType<UploadFileResponse>(metadataResponseObject.Value);

            // Compare values
            Assert.Equal(uploadResponse.Object.ObjectId, metadataResponse.ObjectId);
            Assert.Equal(uploadResponse.Object.QuickNumber, metadataResponse.QuickNumber);
            Assert.Equal(uploadResponse.Object.ObjectName, metadataResponse.ObjectName);
            Assert.Equal(uploadResponse.Object.SizeInByte, metadataResponse.SizeInByte);
        }

        [Fact]
        public async void UploadFileToUnknownLocation_Throws()
        {
            // Mock apis
            var readApiMock = new Mock<IReadApi>();
            var writeApiMock = new Mock<IWriteApi>();
            writeApiMock.Setup(mock =>
                mock.ObjectApi.UploadFile(It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
            ).Throws<InvalidOperationException>();

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            // Query controller
            var response = await controller.UploadFileToUnknownLocation(request);
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async void UploadFileToKnownLocation_Throws()
        {
            // Mock apis
            var readApiMock = new Mock<IReadApi>();
            var writeApiMock = new Mock<IWriteApi>();
            writeApiMock.Setup(mock =>
                mock.ObjectApi.UploadFile(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
            ).Throws<InvalidOperationException>();

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            var response = await controller.UploadFileToKnownLocation(request, "myDirectoryId", 4);
            Assert.IsType<BadRequestResult>(response);
        }
        
        [Fact]
        public async void GetObject_Ok()
        {
            // Mock apis
            var writeApiMock = new Mock<IWriteApi>();
            var readApiMock = new Mock<IReadApi>();
            readApiMock.Setup(mock =>
                mock.ObjectApi.GetObject(
                    It.IsAny<string>(),
                    It.IsAny<uint>())
            ).ReturnsAsync(stream);

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            var response = await controller.GetObject("MyObjId", 4);
            var metadataResponseObject = Assert.IsType<FileStreamResult>(response);
            Assert.Equal(myFile, Stream2Array(metadataResponseObject.FileStream));
        }
        
        [Fact]
        public async void GetObject_Throws()
        {
            // Mock apis
            var readApiMock = new Mock<IReadApi>();
            var writeApiMock = new Mock<IWriteApi>();
            readApiMock.Setup(mock =>
                mock.ObjectApi.GetObject(
                    It.IsAny<string>(), 
                    It.IsAny<uint>())
            ).Throws<InvalidOperationException>();

            var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            var response = await controller.GetObject("MyObjId",  4);
            Assert.IsType<BadRequestResult>(response);
        }

        public async void GetObjectMetadata_Ok()
        {
            var tagMock = new Mock<ITag>();
            tagMock.SetupGet(x => x.Value).Returns("MyTag1");
            tagMock.SetupGet(x => x.Relevance).Returns(0.8f);
            tagMock.SetupGet(x => x.Source).Returns(TagSource.User);


            // Setup dummy response
            var fileMetadataResponse = new Mock<IObjectMetadata>();
            fileMetadataResponse.SetupGet(x => x.ObjectId).Returns("MyFileId");
            fileMetadataResponse.SetupGet(x => x.SizeInByte).Returns((uint)myFile.Length);
            fileMetadataResponse.SetupGet(x => x.ObjectName).Returns(request.Name);
            fileMetadataResponse.SetupGet(x => x.EncryptionKey).Returns("MyEncryptionKey");
            fileMetadataResponse.SetupGet(x => x.Extension).Returns(ExtensionType.Pdf);
            fileMetadataResponse.SetupGet(x => x.Hash).Returns("MyFileHash");
            fileMetadataResponse.SetupGet(x => x.ParentDirectoryIds).Returns(new [] { "MyDirectoryId" });
            fileMetadataResponse.SetupGet(x => x.RevisionNumber).Returns(0);
            fileMetadataResponse.SetupGet(x => x.Revisions).Returns(new[] { 0u });
            fileMetadataResponse.SetupGet(x => x.Tags).Returns(new[] { tagMock.Object });
            fileMetadataResponse.SetupGet(x => x.ThumbnailId).Returns("MyThumbnailId");
            fileMetadataResponse.SetupGet(x => x.UploadDate).Returns(DateTime.MinValue);

            //// Setup dummy response
            //var directoryMetadataResponse = new Mock<IDirectoryMetadata>();
            //directoryMetadataResponse.SetupGet(x => x.ObjectId).Returns("MyDirectoryId");
            //directoryMetadataResponse.SetupGet(x => x.SizeInByte).Returns((uint)myFile.Length);
            //directoryMetadataResponse.SetupGet(x => x.ObjectName).Returns(request.Name);
            //directoryMetadataResponse.SetupGet(x => x.FileChilds).Returns(request.Name);


            //// Mock apis
            //var writeApiMock = new Mock<IWriteApi>();
            //var readApiMock = new Mock<IReadApi>();
            //readApiMock.Setup(mock =>
            //    mock.ObjectApi.GetMetadata(
            //        It.IsAny<string>(),
            //        It.IsAny<uint?>())
            //).ReturnsAsync();

            //var controller = new FilesController(AppSettings, writeApiMock.Object, readApiMock.Object);

            //var response = await controller.GetObject("MyObjId", 4);
            //var metadataResponseObject = Assert.IsType<FileStreamResult>(response);
            //Assert.Equal(myFile, Stream2Array(metadataResponseObject.FileStream));

        }


        public static byte[] Stream2Array(Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
        
    }
}

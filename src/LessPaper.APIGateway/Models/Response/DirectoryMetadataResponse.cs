using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LessPaper.Shared.Interfaces.General;
using LessPaper.Shared.Interfaces.ReadApi.ObjectApi;

namespace LessPaper.APIGateway.Models.Response
{
    public class DirectoryMetadataResponse : MinimalDirectoryMetadataResponse, IDirectoryMetadata
    {
        public DirectoryMetadataResponse(IDirectoryMetadata directoryMetadata) : base(directoryMetadata)
        {
            FileChilds = directoryMetadata.FileChilds.Select(x => new FileMetadataResponse(x)).ToArray();
        }

        /// <inheritdoc />
        [JsonPropertyName("file_childs")]
        public IFileMetadata[] FileChilds { get; }

        /// <inheritdoc />
        [JsonPropertyName("directory_childs")]
        public IMinimalDirectoryMetadata[] DirectoryChilds { get; }
    }
}

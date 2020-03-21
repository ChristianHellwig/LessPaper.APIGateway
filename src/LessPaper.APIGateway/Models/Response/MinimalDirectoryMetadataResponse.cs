using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LessPaper.Shared.Interfaces.General;
using LessPaper.Shared.Interfaces.ReadApi.ObjectApi;

namespace LessPaper.APIGateway.Models.Response
{
    public class MinimalDirectoryMetadataResponse : ObjectResponse, IMinimalDirectoryMetadata
    {
        /// <inheritdoc />
        public MinimalDirectoryMetadataResponse(IMinimalDirectoryMetadata minimalDirectoryMetadata) : base(minimalDirectoryMetadata)
        {
            NumberOfChilds = minimalDirectoryMetadata.NumberOfChilds;
            LatestChangeDate = minimalDirectoryMetadata.LatestChangeDate;
            LatestViewDate = minimalDirectoryMetadata.LatestViewDate;
        }

        /// <inheritdoc />
        [JsonPropertyName("number_of_childs")]
        public uint NumberOfChilds { get; }

        /// <inheritdoc />
        [JsonPropertyName("latest_change_data")]
        public DateTime LatestChangeDate { get; }

        /// <inheritdoc />
        [JsonPropertyName("latest_view_data")]
        public DateTime LatestViewDate { get; }
    }
}

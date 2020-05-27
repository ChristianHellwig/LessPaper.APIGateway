using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LessPaper.Shared.Enums;
using LessPaper.Shared.Interfaces.General;
using LessPaper.Shared.Interfaces.ReadApi.ReadObjectApi;

namespace LessPaper.APIGateway.Models.Response
{
    public class FileMetadataResponse : ObjectResponse, IFileMetadata
    {
        public FileMetadataResponse(IFileMetadata fileMetadata) : base(fileMetadata)
        {
            QuickNumber = fileMetadata.QuickNumber;
            Extension = fileMetadata.Extension;
            UploadDate = fileMetadata.UploadDate;
            EncryptionKey = fileMetadata.EncryptionKey;
            RevisionNumber = fileMetadata.RevisionNumber;
            Hash = fileMetadata.Hash;
            ThumbnailId = fileMetadata.ThumbnailId;
            Revisions = fileMetadata.Revisions;
            ParentDirectoryIds = fileMetadata.ParentDirectoryIds;
            Tags = fileMetadata.Tags.Select(x => new Tag(x)).ToArray();
            Language = fileMetadata.Language;
        }

        /// <inheritdoc />
        [JsonPropertyName("quick_number")]
        public uint QuickNumber { get; }

        /// <inheritdoc />
        [JsonPropertyName("extension")]
        public ExtensionType Extension { get; }

        /// <inheritdoc />
        [JsonPropertyName("upload_date")]
        public DateTime UploadDate { get; }

        /// <inheritdoc />
        [JsonPropertyName("encryption_key")]
        public string EncryptionKey { get; }

        /// <inheritdoc />
        [JsonPropertyName("revision_number")]
        public uint RevisionNumber { get; }

        /// <inheritdoc />
        [JsonPropertyName("hash")]
        public string Hash { get; }

        /// <inheritdoc />
        [JsonPropertyName("thumbnail_id")]
        public string ThumbnailId { get; }

        /// <inheritdoc />
        [JsonPropertyName("revisions")]
        public uint[] Revisions { get; }

        /// <inheritdoc />
        [JsonPropertyName("parent_directory_ids")]
        public string[] ParentDirectoryIds { get; }

        /// <inheritdoc />
        [JsonPropertyName("tags")]
        public ITag[] Tags { get; }

        /// <inheritdoc />
        [JsonPropertyName("language")]
        public DocumentLanguage Language { get; }
    }

    public class Tag : ITag
    {
        public Tag(ITag tag)
        {
            Value = tag.Value;
            Relevance = tag.Relevance;
            Source = tag.Source;
        }

        /// <inheritdoc />
        [JsonPropertyName("value")]
        public string Value { get; }

        /// <inheritdoc />
        [JsonPropertyName("relevance")]
        public float Relevance { get; }

        /// <inheritdoc />
        [JsonPropertyName("source")]
        public TagSource Source { get; }
    }
}

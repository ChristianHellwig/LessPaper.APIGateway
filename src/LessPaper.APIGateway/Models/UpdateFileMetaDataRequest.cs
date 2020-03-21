using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LessPaper.Shared.Interfaces.WriteApi.FileApi;
using Microsoft.AspNetCore.Mvc;

namespace LessPaper.APIGateway.Models
{
    public class UpdateFileMetaDataRequest : IMetadataUpdate
    {
        [Required]
        [JsonPropertyName("object_name")]
        [ModelBinder(Name = "object_name")]
        public string ObjectName { get; set; }
        
        [Required]
        [JsonPropertyName("parent_directory_ids")]
        [ModelBinder(Name = "parent_directory_ids")]
        public string[] ParentDirectoryIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LessPaper.APIGateway.Models
{
    public class UpdateFileMetaDataRequest
    {
        [Required]
        [JsonPropertyName("name")]
        [ModelBinder(Name = "name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("parent_directory_ids")]
        [ModelBinder(Name = "parent_directory_ids")]
        public string[] ParentDirectoryIds { get; set; }
    }
}

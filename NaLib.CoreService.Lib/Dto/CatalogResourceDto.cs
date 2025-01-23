using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NaLib.CoreService.Lib.Dto
{
    public class CatalogResourceDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("resourceType")]
        public string ResourceType { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; } = new List<string>();

        [JsonPropertyName("isBorrowable")]
        public bool IsBorrowable { get; set; }

        [JsonPropertyName("borrowLimitInDays")]
        public int BorrowLimitInDays { get; set; }

        [JsonPropertyName("catalogedBy")]
        public int CatalogedBy { get; set; }

        [JsonPropertyName("borrowStatus")]
        public string BorrowStatus { get; set; }
    }
}
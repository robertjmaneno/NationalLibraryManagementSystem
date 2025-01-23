using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NaLib.CatalogueManagementService.Lib.Data;

public class LibraryResource
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    [JsonPropertyName("id")]
    [BsonIgnore]
    public string IdString
    {
        get => Id.ToString();
        set => Id = string.IsNullOrEmpty(value) ? ObjectId.GenerateNewId() : ObjectId.Parse(value);
    }

    [BsonElement("Title")]
    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Title { get; set; }

    [BsonElement("ResourceType")]
    [Required]
    public string ResourceType { get; set; } 

    [BsonElement("Format")]
    [Required]
    public string Format { get; set; } 

    [BsonElement("Genres")]
    public List<string> Genres { get; set; }

    [BsonElement("Authors")]
    public List<string> Authors { get; set; }

    [BsonElement("Publishers")]
    public List<string> Publishers { get; set; }

    [BsonElement("IsBorrowable")]
    public bool IsBorrowable { get; set; }

    [BsonElement("BorrowRules")]
    public BorrowRule BorrowRules { get; set; } 

    [BsonElement("CatalogedBy")]
    public string CatalogedBy { get; set; }

    [BsonElement("BorrowStatus")]
    public BorrowStatus BorrowStatus { get; set; } 
}


public enum BorrowStatus
{
    Available,
    Borrowed,
    Reserved
}

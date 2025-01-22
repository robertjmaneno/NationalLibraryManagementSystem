using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CatalogueManagementService.Lib.Data
{
    public class ResourceCollection
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } 

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("formats")]
        public List<string> Formats { get; set; } 

        [BsonElement("availability")]
        public AvailabilityInfo Availability { get; set; }

        [BsonElement("borrow_policy")]
        public BorrowPolicyInfo BorrowPolicy { get; set; }

        [BsonElement("cataloged_by")]
        public CatalogedBy CatalogedBy { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }
    }

    public class AvailabilityInfo
    {
        [BsonElement("is_available")]
        public bool IsAvailable { get; set; }

        [BsonElement("in_library_only")]
        public bool InLibraryOnly { get; set; } 
    }

    public class BorrowPolicyInfo
    {
        [BsonElement("limit_days")]
        public int? LimitDays { get; set; }

        [BsonElement("reminders")]
        public ReminderPolicy Reminders { get; set; }
    }

    public class ReminderPolicy
    {
        [BsonElement("almost_due")]
        public int? AlmostDue { get; set; }

        [BsonElement("past_due")]
        public int? PastDue { get; set; }

        [BsonElement("overdue")]
        public int? Overdue { get; set; }
    }

    public class CatalogedBy
    {
        [BsonElement("user_id")]
        public string UserID { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}

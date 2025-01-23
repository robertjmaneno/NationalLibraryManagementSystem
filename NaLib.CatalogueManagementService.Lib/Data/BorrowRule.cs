using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace NaLib.CatalogueManagementService.Lib.Data
{
    public class BorrowRule
    {
        [BsonElement("BorrowLimitInDays")]
        public int BorrowLimitInDays { get; set; }
    }

}

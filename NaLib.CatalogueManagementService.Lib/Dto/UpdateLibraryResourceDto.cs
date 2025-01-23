﻿namespace NaLib.CatalogueManagementService.Lib.Dto
{
    public class UpdateLibraryResourceDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ResourceType { get; set; }
        public string Format { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public bool IsBorrowable { get; set; }
        public int BorrowLimitInDays { get; set; }
        public int CatalogedBy { get; set; }
        public string BorrowStatus { get; set; }

    }
}

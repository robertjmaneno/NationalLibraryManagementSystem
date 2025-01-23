using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NaLib.CatalogueManagementService.Lib.Dto;

namespace NaLib.CatalogueManagementService.Lib.Utils
{
    public class BorrowStatusResolver : IValueResolver<UpdateLibraryResourceDto, LibraryResource, BorrowStatus>
    {
        public BorrowStatus Resolve(UpdateLibraryResourceDto source, LibraryResource destination, BorrowStatus destMember, ResolutionContext context)
        {
            
            return Enum.TryParse(source.BorrowStatus, out BorrowStatus borrowStatus)
                ? borrowStatus
                : BorrowStatus.Available;
        }
    }

}

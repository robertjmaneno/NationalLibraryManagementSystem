using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaLib.CoreService.Lib.Dto;

namespace NaLib.CoreService.Lib.Interfaces
{
   
    
        public interface ICatalogService
        {
            Task<CatalogResourceDto> GetResourceDetailsAsync(string resourceId);
            Task<bool> UpdateResourceStatusAsync(string resourceId, string status);
        }
    }



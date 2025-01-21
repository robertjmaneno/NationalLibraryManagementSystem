using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Dto;

namespace NaLib.CoreService.API.Controllers
{
    public class LendingTransactionController : Controller
    {
        private readonly NaLibCoreServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public LendingTransactionController (NaLibCoreServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


    

    }
}

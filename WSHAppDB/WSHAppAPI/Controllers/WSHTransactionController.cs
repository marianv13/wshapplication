using Microsoft.AspNetCore.Mvc;
using WSHAppDB.Model;
using WSHAppDB.Model.Entities;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WSHAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WSHTransactionController : BaseDataController<WSHTransaction>
    {
        public WSHTransactionController(ILogger<WSHTransactionController> logger, WSHDBContext context) : base(logger, context) { }

        [HttpGet("{min}/{max}")]
        public async Task<int> GetWSHTransactionCountFiltered(int min, int max)
        {
            return await _context.Set<WSHTransaction>().Where(e => e.Sum >= min && e.Sum <= max ).CountAsync();
        }

        [HttpGet("Sumsum")]
        public async Task<int> GetWSHTransactionSumSum()
        {
            return (int)await _context.Set<WSHTransaction>().SumAsync(e => e.Sum);
        }

    }
}
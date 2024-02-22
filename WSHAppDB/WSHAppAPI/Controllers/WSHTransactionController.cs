using Microsoft.AspNetCore.Mvc;
using WSHAppDB.Model;
using WSHAppDB.Model.Entities;

namespace WSHAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WSHTransactionController : BaseDataController<WSHTransaction>
    {
        public WSHTransactionController(ILogger<WSHTransactionController> logger, WSHDBContext context) : base(logger, context) { }

    }
}
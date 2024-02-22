using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WSHAppDB.Model;
using WSHAppDB.Model.Entities;

namespace WSHAppAPI.Controllers
{
    //Az adatok beolvasása a CSV fájlokból
    [Route("api/[controller]")]
    [ApiController]
    public class InitDbController : ControllerBase
    {
        private readonly ILogger<InitDbController> _logger;
        private readonly WSHDBContext _context;

        public InitDbController(ILogger<InitDbController> logger, WSHDBContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        [HttpGet("WSHTransaction")]
        public async Task<IActionResult> InitWSHTransactionAsync()
        {
            var migr = new Migr(_context);
            await migr.InitWSHTransactionAsync(_logger);
            return new OkResult();
        }



        [HttpGet("Migr")]
        public async Task<IActionResult> LoadDataAsync()
        {
            var migr = new Migr(_context);
            await migr.InitWSHTransactionAsync(_logger);
            return new ContentResult() { Content = "Sikerült a betöltés.", ContentType = "text/plain" };
        }
    }

    public class Migr
    {
        private readonly WSHDBContext _context;

        public Migr(WSHDBContext context)
        {
            this._context = context;
        }

        public async Task InitWSHTransactionAsync(ILogger logger)
        {
            //FirstName;LastName;Email;Password;FavSport;FavLeague;FavTeam;Role
            var csv = (await System.IO.File.ReadAllLinesAsync("BaseMigr/WSHTransactions.csv")).Select(e => e.Split(';').ToArray());
            var head = csv.First();

            foreach (var item in csv.Skip(1))
            {
                var transaction = new WSHTransaction();
                transaction.Date = DateTime.Parse(item[0]);
                transaction.Item = item[1];
                transaction.Sum = int.Parse(item[2], CultureInfo.GetCultureInfo("en-US"));

                if (transaction.TransactionId == 0)
                {
                    await _context.AddAsync(transaction);
                }
                await _context.SaveChangesAsync();
            }
            logger.LogInformation($"Init.WSHTransaction:{csv.Count() - 1} db");
        }



        internal static async Task LoadDataAsync(IServiceProvider services)
        {
            using (var serviceScope = services.CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<WSHDBContext>>();
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<WSHDBContext>();

                try
                {
                    var migr = new Migr(dbContext);
                    await migr.InitWSHTransactionAsync(logger);

                    logger.LogInformation("LoadData: OK");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "LoadData hiba");
                }
            }
        }
    }
}
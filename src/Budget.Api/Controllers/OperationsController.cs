using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IMongoDatabase _db;

        public OperationsController(IMongoDatabase db)
        {
            _db = db;
        }

        //
        // GET: /api/budgets/2017/operations

        [HttpGet("~/api/budgets/{budgetId:int}/[controller]")]
        public IActionResult GetByBudgetId(int budgetId)
        {
            return Ok();
        }
    }
}
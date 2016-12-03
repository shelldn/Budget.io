using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;

using ApiOperation = Budget.Api.Models.Operation;
using System.Linq;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IRepository<Operation> _operations;

        public OperationsController(IRepository<Operation> operations)
        {
            _operations = operations;
        }

        //
        // GET: /api/budgets/2017/operations

        [HttpGet("~/api/budgets/{budgetId:int}/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<ApiOperation>), 200)]
        public async Task<IActionResult> GetByBudgetId(int budgetId)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var records = await _operations.QueryAsync(o =>
                o.AccountId == accountId &&
                o.BudgetId == budgetId);

            var operations = records.Select(r => new ApiOperation
            {
                Id = r.Id,
                CategoryId = r.CategoryId,
                Month = r.Month,
                Plan = r.Plan,
                Fact = r.Fact
            });

            return Ok(operations);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Budget.Api.Models.JsonApi;
using ApiOperation = Budget.Api.Models.Operation;

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

        [HttpGet("~/api/budgets/{id:int}/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<ApiOperation>), 200)]
        public async Task<IActionResult> GetByBudgetId(int id)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var records = await _operations.QueryAsync(o =>
                o.AccountId == accountId &&
                o.BudgetId == id);

            var operations = records.Select(r => new ApiOperation
            {
                Id = r.Id,
                BudgetId = r.BudgetId,
                CategoryId = r.CategoryId,
                MonthId = r.Month,
                Plan = r.Plan,
                Fact = r.Fact
            });

            return Ok(operations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiOperation), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _operations.GetByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiOperation), 201)]
        public async Task<IActionResult> Create([FromBody] ApiOperation operation)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var record = new Operation
            {
                AccountId = accountId,
                BudgetId = operation.BudgetId,
                CategoryId = operation.CategoryId,
                Month = operation.MonthId,
                Plan = operation.Plan,
                Fact = operation.Fact
            };

            await _operations.CreateAsync(record);

            operation.Id = record.Id;

            return CreatedAtAction(nameof(GetById), new { id = operation.Id }, operation);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(string id, [FromBody] ResourceObject operation)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var updates = new Dictionary<string, object>(operation.Attributes);

            var props = updates
                .Concat(operation.Relationships.ToDictionary(r => r.Key, r => r.Value.Data))
                .ToDictionary(p => p.Key, p => p.Value);

            await _operations.UpdateAsync(id, props);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _operations.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
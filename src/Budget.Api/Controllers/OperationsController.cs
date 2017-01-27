using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IdentityModel;
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
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

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
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

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
        public async Task<IActionResult> Update(string id, [FromBody] ApiOperation operation)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

            var record = new Operation
            {
                Id = operation.Id,
                AccountId = accountId,
                BudgetId = operation.BudgetId,
                Month = operation.MonthId,
                CategoryId = operation.CategoryId,
                Plan = operation.Plan,
                Fact = operation.Fact
            };

            await _operations.UpdateAsync(id, record);

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
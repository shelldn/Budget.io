using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using Budget.Data.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.JsonPatch;

using ApiOperation = Budget.Api.Models.Operation;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IRepository<Operation> _operations;
        private readonly IPlanPatcher _planPatcher;

        public OperationsController(
            IRepository<Operation> operations,
            IPlanPatcher planPatcher)
        {
            _operations = operations;
            _planPatcher = planPatcher;
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

        //
        // GET: /api/operations/42

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiOperation), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _operations.GetByIdAsync(id));
        }

        //
        // POST: /api/operations

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

        //
        // PATCH: /api/operations/42

        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<ApiOperation> patch)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

            var replace = patch.Operations
              .SingleOrDefault(o =>
                  o.op == "replace" &&
                  o.path == "/plan");

            if (replace == null)
                return BadRequest("You should provide only replace /plan operation");
            
            _planPatcher.Patch(id, accountId, Int32.Parse((string) replace.value));

            return NoContent();
        }

        //
        // DELETE: /api/operations/42

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _operations.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}

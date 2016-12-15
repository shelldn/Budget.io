using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;

using ApiCategory = Budget.Api.Models.Category;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _categories;

        public CategoriesController(IRepository<Category> categories)
        {
            _categories = categories;
        }

        [HttpGet("~/api/budgets/{budgetId:int}/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<ApiCategory>), 200)]
        public async Task<IActionResult> GetByBudgetId(int budgetId)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var records = await _categories.QueryAsync(c =>
                c.BudgetId == budgetId &&
                c.AccountId == accountId);

            var categories = records.Select(r => new ApiCategory
            {
                Id = r.Id,
                BudgetId = r.BudgetId,
                Type = r.Type,
                Name = r.Name
            });

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _categories.GetByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] ApiCategory category)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var record = new Category
            {
                AccountId = accountId,
                BudgetId = category.BudgetId,
                Type = category.Type,
                Name = category.Name
            };

            await _categories.CreateAsync(record);

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApiCategory category)
        {
            var accountId = // User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;
                "5831db9c46c7cae8980e4a56";

            var record = new Category
            {
                Id = id,
                AccountId = accountId,
                BudgetId = category.BudgetId,
                Type = category.Type,
                Name = category.Name
            };

            await _categories.UpdateAsync(id, record);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _categories.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
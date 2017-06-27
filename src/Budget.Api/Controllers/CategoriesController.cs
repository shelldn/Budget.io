using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Budget.Api.Filters;
using Budget.Data;
using Budget.Data.Models;
using IdentityModel;
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

        //
        // GET: /api/budget/2017/categories

        [HttpGet("~/api/budgets/{id:int}/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<ApiCategory>), 200)]
        public async Task<IActionResult> GetByBudgetId(int id)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

            var records = await _categories.QueryAsync(c =>
                c.BudgetId == id &&
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

        //
        // GET: /api/categories/42

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _categories.GetByIdAsync(id));
        }

        //
        // POST: /api/categories

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] ApiCategory category)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

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

        //
        // PATCH: /api/categories/42

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApiCategory category)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

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

        //
        // DELETE: /api/categories/42

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _categories.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}

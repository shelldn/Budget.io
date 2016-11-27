using System.Collections.Generic;
using Budget.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IMongoDatabase _db;

        public CategoriesController(IMongoDatabase db)
        {
            _db = db;
        }

        [HttpGet("~/api/budgets/{budgetId:int}/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        public IActionResult GetByBudgetId(int budgetId)
        {
            var accountId = User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value;

            var categories = _db
                .GetCollection<Category>("categories")
                .Find(c =>
                    c.BudgetId == budgetId &&
                    c.AccountId == accountId);

            return Ok(categories.ToList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        public IActionResult GetById(string id)
        {
            return Ok(new Category { Id = id });
        }

        [HttpPost]
        [ProducesResponseType(typeof(Category), 201)]
        public IActionResult Create(Category category)
        {
            return CreatedAtAction("GetById", new { category.Id });
        }

        [HttpPatch("{id}")]
        public IActionResult Update(Category category)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(string id)
        {
            return NoContent();
        }
    }
}
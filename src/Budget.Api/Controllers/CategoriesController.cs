using System;
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

        //
        // GET: /api/budgets/2017/categories

        [HttpGet("~/api/budgets/{budgetId:int}/[controller]")]
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
    }
}
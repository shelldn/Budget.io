using System;
using System.Linq;
using Budget.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        private readonly IMongoDatabase _db;

        public BudgetsController(IMongoDatabase db)
        {
            _db = db;
        }

        //
        // GET: /api/budgets/2016

        [HttpGet("{id:int}")]
        public object GetById(int id)
        {
            var budgets = _db.GetCollection<BudgetRecord>("budgets");

            var accountId = Int32.Parse(User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value);

            budgets.Find(b => b.Year == id && b.AccountId == accountId).Single();

            return new
            {
                data = new
                {
                    type = "budgets",
                    id,
                    relationships = new
                    {
                        categories = new
                        {
                            links = new
                            {
                                related = Url.Action("GetAll", "BudgetCategories", new { budgetId = id })
                            }
                        }
                    }
                },
            };
        }
    }
}
using System;
using System.Linq;
using Budget.Api.Models;
using Budget.Api.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Route("api/budgets/{budgetId:int}/categories")]
    public class BudgetCategoriesController : Controller
    {
        private readonly IMongoDatabase _db;

        public BudgetCategoriesController(IMongoDatabase db)
        {
            _db = db;
        }

        //
        // GET: /api/budgets/2016/categories

        [HttpGet]
        public object GetAll(int budgetId)
        {
            var budgets = _db.GetCollection<BudgetRecord>("budgets");
            var operations = _db.GetCollection<Operation>("operations");

            var accountId = Int32.Parse(User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value);

            var budget = budgets.Find(b => b.Year == budgetId && b.AccountId == accountId).Single();

            return new
            {
                data = budget.Categories.Select(c => new
                {
                    type = "categories",
                    id = c.Id,
                    links = new
                    {
                        self = Url.Action("Update", new { budgetId, id = c.Id })
                    },
                    attributes = new
                    {
                        name = c.Name,
                        type = c.Type
                    },
                    relationships = new
                    {
                        operations = new
                        {
                            data = operations.Find(o => o.BudgetId == budgetId && o.CategoryId == c.Id).ToList().Select(o => new
                            {
                                type = "operations",
                                o.Id
                            })
                        }
                    }
                }),

                included = operations.Find(o => o.BudgetId == budgetId).ToList().Select(o => new
                {
                    type = "operations",
                    o.Id,
                    attributes = new
                    {
                        o.Plan,
                        o.Fact
                    },
                    relationships = new
                    {
                        month = new
                        {
                            data = new
                            {
                                type = "months",
                                id = o.Month
                            }
                        },
                        category = new
                        {
                            data = new
                            {
                                type = "category",
                                id = o.CategoryId
                            }
                        }
                    }
                })
            };
        }

        //
        // PATCH: /api/budgets/2016/categories/1

        [HttpPatch("{id:int}")]
        public IActionResult Update(int budgetId, int id, [FromBody] CategoryPatch patch)
        {
            var budgets = _db.GetCollection<BudgetRecord>("budgets");

            var accountId = Int32.Parse(User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value);

            var filter = budgets.Find(
                b => b.Year == budgetId &&
                b.AccountId == accountId &&
                b.Categories.Any(c => c.Id == id)
            ).Filter;

            var update = Builders<BudgetRecord>.Update.Set(b => b.Categories.ElementAt(-1).Name, patch.Name);

            budgets.UpdateOne(filter, update);

            return NoContent();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Budget.Api.Models;
using Budget.Api.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Route("api/budgets/{budgetId:int}/categories")]
    public class BudgetCategoriesController : Controller
    {
        private readonly IConfiguration _config;

        public BudgetCategoriesController(IConfiguration config)
        {
            _config = config;
        }

        //
        // GET: /api/budgets/2016/categories

        [HttpGet]
        public object GetAll(int budgetId)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient(_config.GetConnectionString("DefaultConnection"));
            var db = client.GetDatabase("budgetio");
            var budgets = db.GetCollection<BudgetRecord>("budgets");

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
                    }
                })
            };
        }

        //
        // PATCH: /api/budgets/2016/categories/1

        [HttpPatch("{id:int}")]
        public IActionResult Update(int budgetId, int id, [FromBody] CategoryPatch patch)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient(_config.GetConnectionString("DefaultConnection"));
            var db = client.GetDatabase("budgetio");
            var budgets = db.GetCollection<BudgetRecord>("budgets");

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
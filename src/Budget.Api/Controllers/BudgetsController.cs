using System.Collections.Generic;
using System.Linq;
using Budget.Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        //
        // GET: /api/budgets

        [HttpGet]
        public IEnumerable<BudgetSummary> GetAll()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient("mongodb://shelldn-ubuntu.westeurope.cloudapp.azure.com:27027/");
            var db = client.GetDatabase("budgetio");
            var budgets = db.GetCollection<BudgetSummary>("budgets");

            return budgets
                .Find(new BsonDocument())
                .ToList();
        }

        //
        // GET: /api/budgets/2016

        [HttpGet("{id:int}")]
        public object GetById(int id)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient("mongodb://shelldn-ubuntu.westeurope.cloudapp.azure.com:27027/");
            var db = client.GetDatabase("budgetio");
            var budgets = db.GetCollection<BudgetRecord>("budgets");

            var budget = budgets.Find(b => b.Year == id && b.AccountId == 1).Single();

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
                            data = budget.Categories.Select(c => new
                            {
                                type = "categories",
                                id = c.Id
                            })
                        }
                    }
                },

                included = budget.Categories.Select(c => new
                {
                    type = "categories",
                    id = c.Id,
                    attributes = new
                    {
                        type = c.Type,
                        name = c.Name
                    }
                })
            };
        }
    }
}
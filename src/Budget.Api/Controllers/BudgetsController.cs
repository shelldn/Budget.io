using System.Collections.Generic;
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
    }
}
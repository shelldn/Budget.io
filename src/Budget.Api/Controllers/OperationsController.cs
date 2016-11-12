using System;
using Budget.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Linq;

namespace Budget.Api.Controllers
{
    [Route("api/budgets/{budgetId:int}/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IConfiguration _config;

        public OperationsController(IConfiguration config)
        {
            _config = config;
        }

        //
        // POST: /api/budgets/2016/operations

        [HttpPost(Name = "GetOperation")]
        public IActionResult Create(int budgetId, [FromBody] Operation operation)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient(_config.GetConnectionString("DefaultConnection"));
            var db = client.GetDatabase("budgetio");
            var operations = db.GetCollection<Operation>("operations");

            operation.BudgetId = budgetId;

            operations.InsertOne(operation);

            return CreatedAtRoute("GetOperation", new
            {
                data = new
                {
                    type = "operations",
                    id = operation.Id,
                    attributes = new
                    {
                        plan = operation.Plan,
                        fact = operation.Fact
                    }
                }
            });
        }
    }
}
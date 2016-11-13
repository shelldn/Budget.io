using Budget.Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Route("api/budgets/{budgetId:int}/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IMongoDatabase _db;

        public OperationsController(IMongoDatabase db)
        {
            _db = db;
        }

        //
        // POST: /api/budgets/2016/operations

        [HttpPost(Name = "GetOperation")]
        public IActionResult Create(int budgetId, [FromBody] Operation operation)
        {
            var operations = _db.GetCollection<Operation>("operations");

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

        //
        // PATCH: /api/budgets/2016/operations/58277bb5e209b730805c1c80

        [HttpPatch("{id}")]
        public IActionResult Update(int budgetId, string id, [FromBody] Operation operation)
        {
            var operations = _db.GetCollection<Operation>("operations");

            operations.UpdateOne(operations.Find(o => o.BudgetId == budgetId && o.Id == id).Filter,
                Builders<Operation>.Update.Set(o => o.Plan, operation.Plan).Set(o => o.Fact, operation.Fact));

            return NoContent();
        }
    }
}
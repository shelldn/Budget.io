using System;
using System.Collections.Generic;
using System.Linq;
using Budget.Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        private readonly IConfiguration _config;

        public BudgetsController(IConfiguration config)
        {
            _config = config;
        }

        //
        // GET: /api/budgets/2016

        [HttpGet("{id:int}")]
        public object GetById(int id)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            var client = new MongoClient(_config.GetConnectionString("DefaultConnection"));
            var db = client.GetDatabase("budgetio");
            var budgets = db.GetCollection<BudgetRecord>("budgets");

            var accountId = Int32.Parse(User.Claims.Single(c => c.Type == JwtClaimTypes.Subject).Value);

            var budget = budgets.Find(b => b.Year == id && b.AccountId == accountId).Single();

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
using Budget.Api.Services;
using Microsoft.AspNetCore.Mvc;
using ApiBudget = Budget.Api.Models.Budget;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        private readonly IMonthGenerator _monthGenerator;

        public BudgetsController(IMonthGenerator monthGenerator)
        {
            _monthGenerator = monthGenerator;
        }

        //
        // GET: /api/budgets/2017

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            System.Console.WriteLine(id);
            return Ok(new ApiBudget
            {
                Id = id,
                Months = _monthGenerator.GenerateYear()
            });
        }
    }
}

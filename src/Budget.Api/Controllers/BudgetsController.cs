using Microsoft.AspNetCore.Mvc;
using ApiBudget = Budget.Api.Models.Budget;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(new ApiBudget
            {
                Id = id
            });
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class BudgetsController : Controller
    {
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(new
            {
                data = new
                {
                    type = "budget",
                    id,
                    relationships = new
                    {
                        categories = new
                        {
                            links = new
                            {
                                related = Url.Action("GetByBudgetId", "Categories", new {budgetId = id})
                            }
                        }
                    }
                }
            });
        }
    }
}
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class MonthsController : Controller
    {
        [HttpGet]
        public object GetAll()
        {
            return new
            {
                data = Enumerable.Range(1, 12).Select(id => new { type = "months", id })
            };
        }
    }
}
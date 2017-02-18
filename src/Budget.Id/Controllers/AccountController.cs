using Budget.Id.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Id.Controllers
{
    public class AccountController : Controller
    {
        //
        // POST: /register

        [HttpPost("register")]
        public IActionResult Register([FromBody] AccountInfo info)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}
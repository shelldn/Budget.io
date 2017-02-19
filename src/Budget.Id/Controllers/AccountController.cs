using Budget.Data;
using Budget.Data.Models;
using Budget.Id.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Id.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<Account> _accounts;

        public AccountController(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        //
        // POST: /register

        [HttpPost("register")]
        public IActionResult Register([FromBody] AccountInfo info)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _accounts.CreateAsync(new Account
            {
                UserName = info.UserName,
                Password = info.Password
            }).Wait();

            return Ok();
        }
    }
}
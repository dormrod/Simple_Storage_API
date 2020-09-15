using Microsoft.AspNetCore.Mvc;
using Storage.API.Models;
using Storage.API.Services;

namespace Storage.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserRegisterController : Controller
    {
        private AuthRepository authRepository;

        public UserRegisterController()
        {
            authRepository = new AuthRepository();
        }

        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            var error = authRepository.RegisterUser(username, password);
            if (error) return BadRequest("User not added: username may already exist");
            else return Ok("User successfully added");
        }

    }
}

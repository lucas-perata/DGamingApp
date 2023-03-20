using Microsoft.AspNetCore.Mvc;
using DGamingApp.Interfaces;

namespace DGamingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/users
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

       
    }
}

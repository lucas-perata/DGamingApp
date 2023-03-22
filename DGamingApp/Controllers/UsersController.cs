using Microsoft.AspNetCore.Mvc;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DGamingApp.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
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

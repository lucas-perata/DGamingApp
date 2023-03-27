using Microsoft.AspNetCore.Mvc;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DGamingApp.Dto;

namespace DGamingApp.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetMembersAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToReturn = _mapper.Map<MemberDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userRepository.GetMember(name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }


    }
}

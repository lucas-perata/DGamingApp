using Microsoft.AspNetCore.Mvc;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DGamingApp.Dto;
using System.Security.Claims;

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

        [HttpPut] 
        public async Task<IActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get username from token (JWT)
            if (username == null) return BadRequest("Not found");

            var user = await _userRepository.GetUserByName(username);

            if (user == null) return BadRequest("Not found"); 

            _mapper.Map(memberUpdateDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }


    }
}

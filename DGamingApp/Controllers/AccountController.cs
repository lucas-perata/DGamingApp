using AutoMapper;
using DGamingApp.Data;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DGamingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) 
        {
            _userManager = userManager; 
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password); 

            if (!result.Succeeded) return BadRequest(result.Errors); 

            var roleResult = await _userManager.AddToRoleAsync(user, "Member"); 

            if(!roleResult.Succeeded) return BadRequest(result.Errors); 

            var userRegistered = new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Gender = user.Gender, 
                Token = await _tokenService.CreateToken(user)
            };

            return(userRegistered);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password); 

            if(!result) return BadRequest("Invalid password"); 

            return new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                Token = await  _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        private async Task<bool> UserExists(string username) {

            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower()); 
        }
    }
}

﻿    using AutoMapper;
using DGamingApp.Data;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DGamingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        public readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper) 
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await _context.Users.AnyAsync(u => u.UserName == registerDto.Username)) return BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDto);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var userRegistered = new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Gender = user.Gender, 
                Token = _tokenService.CreateToken(user)
            };

            return(userRegistered);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(u => u.UserName == loginDto.Username );

            if (user == null) return Unauthorized("Invalid username");

            return new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                Token =  _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url
            };
        }
    }
}

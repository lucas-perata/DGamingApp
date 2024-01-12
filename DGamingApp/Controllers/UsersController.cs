using Microsoft.AspNetCore.Mvc;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DGamingApp.Dto;
using System.Security.Claims;
using DGamingApp.Entities;
using DGamingApp.Extensions;
using DGamingApp.Helpers;

namespace DGamingApp.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetAll([FromQuery]UserParams userParams)
        {
            var currentUser = await _uow.UserRepository.GetUserByName(User.GetUsername()); 
            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender)) 
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _uow.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, 
            users.TotalCount, users.TotalPages));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _uow.UserRepository.GetUserById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToReturn = _mapper.Map<MemberDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _uow.UserRepository.GetMember(name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPut] 
        public async Task<IActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUsername(); // Get username from token (JWT)
            if (username == null) return BadRequest("Not found");

            var user = await _uow.UserRepository.GetUserByName(username);

            if (user == null) return BadRequest("Not found"); 

            _mapper.Map(memberUpdateDto, user);

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();

            if (username == null) return NotFound();

            var user = await _uow.UserRepository.GetUserByName(username); 

            if(user == null) return NotFound();  

            var result = await _photoService.AddPhotoAsync(file); 

            if(result.Error != null) return BadRequest(result.Error.Message); 

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri, 
                PublicId = result.PublicId
            }; 

            if(user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo); 

            if(await _uow.Complete())
            {
                return CreatedAtAction(nameof(GetUserByName), new {name = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Error adding the photo"); 
        } 

        [HttpPut("set-main-photo/{photoId}")] 

        public async Task<ActionResult> SetMainPhoto(int photoId) 
        {
            var user = await _uow.UserRepository.GetUserByName(User.GetUsername()); 

            if (user == null) return NotFound(); 

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound(); 

            if (photo.IsMain) return BadRequest("This is already main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain); 

            if (currentMain != null) currentMain.IsMain = false; 
            photo.IsMain = true; 

            if(await _uow.Complete()) return NoContent(); 

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")] 
        public async Task<ActionResult> DeletePhoto(int photoId) 
        {
            var user = await _uow.UserRepository.GetUserByName(User.GetUsername()); 

            if (user == null) return NotFound(); 

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId); 

            if (photo == null) return NotFound(); 

            if (photo.IsMain) return BadRequest("You cannot delete your main photo"); 

            if (photo.PublicId != null) 
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId); 
                if(result.Error != null) return BadRequest(result.Error.Message); 
            }

            user.Photos.Remove(photo); 

            if(await _uow.Complete()) return Ok();

            return BadRequest("There was a problem deleting the photo");
        }

    }
}

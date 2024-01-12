using AutoMapper;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Extensions;
using DGamingApp.Helpers;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DGamingApp.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper; 
        private readonly IUnitOfWork _uow;
        public MessagesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername(); 

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot message yourself");  
            
            var sender = await _uow.UserRepository.GetUserByName(username);  
            var recipient = await _uow.UserRepository.GetUserByName(createMessageDto.RecipientUsername); 

            if (recipient == null) return NotFound(); 

            var message = new Message
            {
                Sender = sender, 
                Recipient = recipient,
                SenderUsername = sender.UserName, 
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            }; 

            _uow.MessageRepository.AddMessage(message);  

            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message)); 

            return BadRequest("Failed to send"); 
        }

        [HttpGet] 

        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams) 
        {
            messageParams.Username = User.GetUsername();

            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams); 

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages)); 

            return messages; 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) 
        {
            var username = User.GetUsername(); 

            var message = await _uow.MessageRepository.GetMessage(id);  

            if(message.SenderUsername != username && message.RecipientUsername != username) 
                return Unauthorized();  

            if(message.SenderUsername == username) message.SenderDeleted = true; 
            if(message.RecipientUsername == username) message.RecipientDeleted = true; 

            if(message.SenderDeleted && message.RecipientDeleted) 
            {
                _uow.MessageRepository.DeleteMessage(message); 
            }

            if(await _uow.Complete()) return Ok(); 

            return BadRequest("Problem deleting the message"); 
        }
    }
}
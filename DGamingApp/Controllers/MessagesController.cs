using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUserRepository _userRepository; 
        private readonly IMessageRepository _messageRepository; 
        private readonly IMapper _mapper; 
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository; 
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername(); 

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot message yourself");  
            
            var sender = await _userRepository.GetUserByName(username);  
            var recipient = await _userRepository.GetUserByName(createMessageDto.RecipientUsername); 

            if (recipient == null) return NotFound(); 

            var message = new Message
            {
                Sender = sender, 
                Recipient = recipient,
                SenderUsername = sender.UserName, 
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            }; 

            _messageRepository.AddMessage(message);  

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message)); 

            return BadRequest("Failed to send"); 
        }

        [HttpGet] 

        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams) 
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessagesForUser(messageParams); 

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages)); 

            return messages; 
        }
    }
}
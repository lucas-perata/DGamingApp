using System;
using AutoMapper;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Extensions;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SQLitePCL;

namespace DGamingApp.SignalIR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;

        public MessageHub(IUnitOfWork uow, IMapper mapper, IHubContext<PresenceHub> presenceHub)
        {
            _uow = uow;
            _mapper = mapper;
            _presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext(); 
            var otherUser = httpContext.Request.Query["user"]; 
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName); 
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _uow.MessageRepository.GetMessageThread(Context.User.GetUsername(), otherUser); 

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages); 
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup(); 
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);   
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername(); 

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("Cannot send a message to yourself"); 
            
            var sender = await _uow.UserRepository.GetUserByName(username);  
            var recipient = await _uow.UserRepository.GetUserByName(createMessageDto.RecipientUsername); 

            if (recipient == null) throw new HubException("User not found"); 

            var message = new Message
            {
                Sender = sender, 
                Recipient = recipient,
                SenderUsername = sender.UserName, 
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            }; 

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _uow.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow; 
            }
            else 
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if(connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", 
                        new {username = sender.UserName, knownAs = sender.KnownAs});
                }
            }

            _uow.MessageRepository.AddMessage(message);  

            if (await _uow.Complete()) 
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        
        }

        private string GetGroupName(string caller, string other) 
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0; 
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}"; 
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _uow.MessageRepository.GetMessageGroup(groupName); 
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if(group == null)
            {
                group = new Group(groupName); 
                _uow.MessageRepository.AddGroup(group); 
            }

            group.Connections.Add(connection);

            if (await _uow.Complete()) return group; 

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _uow.MessageRepository.GetGroupForConnection(Context.ConnectionId); 
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _uow.MessageRepository.RemoveConnection(connection); 
            
            if (await _uow.Complete()) return group; 

            throw new HubException("Failed to remove from group"); 
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DGamingApp.Data;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Helpers;
using DGamingApp.Interfaces;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DGamingApp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper; 
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
            
        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                            .OrderByDescending(x => x.MessageSent)
                            .AsQueryable(); 
            
            query = messageParams.Container switch 
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null) 
            }; 

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider); 

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize); 
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string RecipientUsername)
        {
            var messages = await _context.Messages
                            .Include(u => u.Sender).ThenInclude(p => p.Photos)
                            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                            .Where(
                                m => m.RecipientUsername == currentUserName && 
                                m.SenderUsername == RecipientUsername ||
                                m.RecipientUsername == RecipientUsername && 
                                m.SenderUsername == currentUserName
                             )
                             .OrderByDescending(m => m.MessageSent)
                             .ToListAsync(); 

            var unreadMessages = messages.Where(m => m.DateRead == null 
            && m.RecipientUsername == currentUserName).ToList(); 

            if (unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(); 
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages); 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Helpers;

namespace DGamingApp.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message); 
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);  
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams); 
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string RecipientUsername); 
        Task<bool> SaveAllAsync(); 
    }
}
using API.Data_Layer.DTOs;
using API.Data_Layer.Models;
using API.Helpers;

namespace API.Repository_Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection?> GetConnection(string connectionId);
        Task<Group?> GetMesssageGroup(string groupName);
        Task<Group?> GetGroupForConnection(string connectionId);
    }
}

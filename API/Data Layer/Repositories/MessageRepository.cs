﻿using API.Data_Layer.DTOs;
using API.Data_Layer.Models;
using API.Helpers;
using API.Repository_Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Connection = API.Data_Layer.Models.Connection;

namespace API.Data_Layer.Repositories
{
    public class MessageRepository(DataDbContext context, IMapper mapper) : IMessageRepository
    {
        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Connection?> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDTO>> GetMessageForUser(MessageParams messageParams)
        {
            var query = context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.Username &&
                x.RecipientDeleted == false),

                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username &&
                x.SenderDeleted == false),

                _ => query.Where(x => x.Recipient.UserName == messageParams.Username 
                && x.MessageRead == null 
                && x.RecipientDeleted == false)
            };

            var messages = query.ProjectTo<MessageDTO>(mapper.ConfigurationProvider);

            return await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var query = context.Messages
                .Where(x =>

                    x.RecipientUsername == currentUsername
                        && x.RecipientDeleted == false
                        && x.SenderUsername == recipientUsername

                    ||

                    x.SenderUsername == currentUsername
                        && x.SenderDeleted == false
                        && x.RecipientUsername == recipientUsername)

                .OrderBy(x => x.MessageSent)
                .AsQueryable();

            var unreadMessages = query
                .Where(x => x.MessageRead == null &&
                x.RecipientUsername == currentUsername).ToList();

            if(unreadMessages.Count != 0)
            {
                unreadMessages.ForEach(x => x.MessageRead = DateTime.UtcNow);
            }

            return await query.ProjectTo<MessageDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<Group?> GetMesssageGroup(string groupName)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }
    }
}

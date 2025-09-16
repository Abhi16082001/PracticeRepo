using Chatting_Calling.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chatting_Calling.Hubs
{
    public class ChattingHub :Hub
    {
        private static List<string> AllUsers;
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private static readonly ConcurrentDictionary<string, List<string>> UserConnections =
        new ConcurrentDictionary<string, List<string>>();

        // Example: all users in the system (you can get from DB)
        

        public override Task OnConnected()
        {
            var userId = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.AddOrUpdate(userId,
                    new List<string> { Context.ConnectionId },
                    (key, existingList) => { existingList.Add(Context.ConnectionId); return existingList; });
            }

            UpdateClientsUserStatus();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userId) && UserConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(Context.ConnectionId);
                if (!connections.Any())
                    UserConnections.TryRemove(userId, out _);
            }

            UpdateClientsUserStatus();
            return base.OnDisconnected(stopCalled);
        }

        private void UpdateClientsUserStatus()
        {
             AllUsers = _db.Users.Select(x => x.Id).ToList();
            var onlineUsers = UserConnections.Keys.ToList();

            // Build user status object: { Username, IsOnline }
            var userStatusList = AllUsers.Select(u => new
            {
                Username = u,
                IsOnline = onlineUsers.Contains(u)
            }).ToList();

            Clients.All.updateUserList(userStatusList);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {
            var fromUserId = Context.User.Identity.Name;
            var chatMessage = new ChatMessage
            {
                SenderId = fromUserId,
                ReceiverId = toUserId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            _db.ChatMessages.Add(chatMessage);
            _db.SaveChanges();

            if (UserConnections.TryGetValue(toUserId, out var connections))
            {
                foreach (var connId in connections)
                {
                    Clients.Client(connId).receiveMessage(fromUserId, message, chatMessage.SentAt);
                }
            }

            if (UserConnections.TryGetValue(fromUserId, out var senderConnections))
            {
                foreach (var connId in senderConnections)
                {
                    Clients.Client(connId).receiveMessage(fromUserId, message, chatMessage.SentAt);
                }
            }
        }

    }
}
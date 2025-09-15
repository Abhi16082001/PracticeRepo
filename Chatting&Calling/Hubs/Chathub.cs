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
    public class Chathub :Hub
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        // Keeps track of users
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public override Task OnConnected()
        {
            var userId = Context.User.Identity.Name; // Extracted from JWT
            UserConnections[userId] = Context.ConnectionId;
            return base.OnConnected();
        }

        // Send text message
        //public void SendPrivateMessage(string toUserId, string message)
        //{
        //    var fromUserId = Context.ConnectionId; // replace with actual user mapping

        //    var chatMessage = new ChatMessage
        //    {
        //        SenderId = fromUserId,
        //        ReceiverId = toUserId,
        //        Content = message,
        //        SentAt = DateTime.UtcNow
        //    };

        //    _db.ChatMessages.Add(chatMessage);
        //    _db.SaveChanges();

        //    // Send to receiver
        //    Clients.Client(toUserId).receiveMessage(fromUserId, message, chatMessage.SentAt);

        //    // Echo to sender
        //    Clients.Caller.receiveMessage(fromUserId, message, chatMessage.SentAt);
        //}



        public void SendPrivateMessage(string toUserId, string message)
        {
            var fromUserId = Context.User.Identity.Name; // JWT user

            var chatMessage = new ChatMessage
            {
                SenderId = fromUserId,
                ReceiverId = toUserId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            _db.ChatMessages.Add(chatMessage);
            _db.SaveChanges();

            if (UserConnections.TryGetValue(toUserId, out string connectionId))
            {
                Clients.Client(connectionId).receiveMessage(fromUserId, message, chatMessage.SentAt);
            }

            Clients.Caller.receiveMessage(fromUserId, message, chatMessage.SentAt);
        }


        // WebRTC signaling
        public void SendOffer(string toUserId, string sdpOffer)
        {
            var fromUserId = Context.ConnectionId;
            Clients.Client(toUserId).receiveOffer(fromUserId, sdpOffer);
        }

        public void SendAnswer(string toUserId, string sdpAnswer)
        {
            var fromUserId = Context.ConnectionId;
            Clients.Client(toUserId).receiveAnswer(fromUserId, sdpAnswer);
        }

        public void SendIceCandidate(string toUserId, string candidate)
        {
            var fromUserId = Context.ConnectionId;
            Clients.Client(toUserId).receiveCandidate(fromUserId, candidate);
        }

    }
}
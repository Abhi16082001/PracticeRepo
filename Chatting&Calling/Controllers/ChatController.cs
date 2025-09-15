using Chatting_Calling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chatting_Calling.Controllers
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        [Route("history/{user1}/{user2}")]
        public IHttpActionResult GetChatHistory(string user1, string user2)
        {
            var messages = _db.ChatMessages
                .Where(m =>
                    (m.SenderId == user1 && m.ReceiverId == user2) ||
                    (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.SentAt)
                .ToList();

            return Ok(messages);
        }

    }
}

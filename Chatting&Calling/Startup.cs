using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Chatting_Calling.Startup))]
namespace Chatting_Calling
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map SignalR hubs at /signalr
            app.MapSignalR(); // register hubs
        }
    }
}
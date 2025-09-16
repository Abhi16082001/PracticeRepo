using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Threading.Tasks;


[assembly: OwinStartup(typeof(Chatting_Calling.Startup))]
namespace Chatting_Calling
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var key = Encoding.ASCII.GetBytes("ThisIsASuperLongSecretKey1234567890!@#");

            // ✅ Enable JWT Bearer Authentication
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    NameClaimType = ClaimTypes.NameIdentifier
                },
                Provider = new OAuthBearerAuthenticationProvider
                {
                    // 👇 This lets SignalR read token from query string ?access_token=xxx
                    OnRequestToken = context =>
                    {
                        var token = context.Request.Query.Get("access_token");
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.FromResult(0);
                    }
                }
            });




            // Map SignalR hubs at /signalr
            app.MapSignalR(); // register hubs
        }
    }
}
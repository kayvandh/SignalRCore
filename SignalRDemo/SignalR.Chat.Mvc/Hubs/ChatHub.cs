using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR.Chat.Mvc.Data;
using SignalR.Chat.Mvc.Models;

namespace SignalR.Chat.Mvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatHub(UserManager<ChatUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task BroadCastFromClient(string message)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(Context.User);

                var msg = new Message()
                {
                    MessageBody = message,
                    MessageDatetime = DateTime.Now,
                    FromUser = currentUser
                };

                await _context.Messages.AddAsync(msg);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync(
                    "Broadcast",
                    new
                    {
                        messageBody = msg.MessageBody,
                        fromUser = currentUser.Email,
                        messageDatetime = msg.MessageDatetime.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
                    }
                );

            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("HubError", new {error = e.Message});
            }
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);

            await Clients.All.SendAsync("UserConnected", new
            {
                connectionId = Context.ConnectionId,
                connectionDatetime = DateTime.Now,
                messageDatetime = DateTime.Now.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture),
                userName = currentUser.Email
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("User|Disconnected",
                $"User disconnected, ConnectionId: {Context.ConnectionId}");
        }
    }
}

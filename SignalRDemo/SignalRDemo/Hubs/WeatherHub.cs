﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Hubs
{
    public class WeatherHub : Hub
    {
        public async Task BroadcastFromClient(string message)
        {
            await Clients.All.SendAsync("Broadcast", message);
        }
    }
}

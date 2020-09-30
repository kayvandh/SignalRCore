using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalR.Chat.Mvc.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {

        public ChatController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

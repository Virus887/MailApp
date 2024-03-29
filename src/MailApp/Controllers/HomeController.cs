﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ErrorViewModel = MailApp.Models.ErrorViewModel;

namespace MailApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            RedirectToAction("Index", "Message");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
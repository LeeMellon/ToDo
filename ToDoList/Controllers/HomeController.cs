using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {

      [HttpGet("/")]
      public ActionResult Index()
      {
          return View();
      }

      [HttpGet("/success")]
      public ActionResult Success()
      {
          return View();
      }


    }
}

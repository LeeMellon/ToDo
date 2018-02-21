using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;
namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/items")]
        public ActionResult Index()
        {
          List<Item> allItems = Item.GetAll();
          return View(allItems);
        }

        [HttpGet("/items/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpGet("/items/urgent")]
        public ActionResult OrderByDesc()
        {
          List<Item> allItems = Item.DateDesc();
          return View("Index", allItems);
        }

        [HttpPost("/items")]
        public ActionResult Create()
        {
          string formDate = Request.Form["due-date"];
          DateTime newDate = DateTime.Parse(formDate);
          Item newItem = new Item (Request.Form["new-item"], newDate);
          newItem.Save();
          List<Item> allItems = Item.GetAll();
          return View("Index", allItems);
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
            Item.DeleteAll();
            return View();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;
namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/items/{id}/new")]
        public ActionResult CreateForm(int id)
        {
          Category thisCategory = Category.Find(id);
            return View(thisCategory);
        }

        [HttpPost("/items/new")]
        public ActionResult Create()
        {
          int formId = Convert.ToInt32(Request.Form["category_id"]);
          string formDate = Request.Form["due-date"];
          DateTime newDate = DateTime.Parse(formDate);
          Item newItem = new Item (Request.Form["new-item"], newDate, formId);
          newItem.Save();
          return RedirectToAction("Index","categories");
        }

        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
          Item thisItem = Item.Find(id);
          return View(thisItem);
        }

        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id)
        {
          Item thisItem = Item.Find(id);
          thisItem.Edit(Request.Form["newname"]);
          return RedirectToAction("index", "categories");
        }

        [HttpPost("/items/{id}/delete_all")]
        public ActionResult DeleteAll(int id)
        {
            Item.DeleteAll(id);
            return RedirectToAction("index", "categories");
        }

        [HttpPost("/items/{id}/delete")]
        public ActionResult DeleteItem(int id)
        {
          Item thisItem = Item.Find(id);
          int categoryId = thisItem.GetCategoryId();
          Item.DeleteItem(id);
          return RedirectToAction("CategoryDetails", "categories", new {Id = thisItem.GetCategoryId()});
        }

    }
}

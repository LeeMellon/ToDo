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
          Category thisCategory = Category.Find(formId);
          string formDate = Request.Form["due-date"];
          DateTime newDate = DateTime.Parse(formDate);
          Item newItem = new Item (Request.Form["new-item"], newDate);
          newItem.Save();
          thisCategory.AddItem(newItem);
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
            Item.DeleteAllByCategory(id);
            return RedirectToAction("index", "categories");
        }

        [HttpGet("/items/{id}/categories/{catId}/delete")]
        public ActionResult DeleteItemCategory(int id, int catId)
        {
          Item thisItem = Item.Find(id);
          thisItem.DeleteItemCategory(catId);

          // return RedirectToRoute("/category/{id}", new {id = catId});
          return RedirectToAction("CategoryDetails", "Categories", new {id = catId});
        }

    }
}

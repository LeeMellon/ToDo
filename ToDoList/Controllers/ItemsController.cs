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


      [HttpGet("/items/{id}")]
      public ActionResult Details(int id)
      {
        Item thisItem = Item.Find(id);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Category> itemCategories = thisItem.GetCategories();
        List<Category> allCategories = Category.GetAll();
        model.Add("item", thisItem);
        model.Add("itemCategories", itemCategories);
        model.Add("allCategories", allCategories);
        return View(model);
      }

      //ADD CATEGORY TO TASK
      [HttpPost("/items/{itemId}/categories/new")]
      public ActionResult AddCategory(int itemId)
      {
        Item item = Item.Find(itemId);
        Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
        item.AddCategory(category);
        return RedirectToAction("Success", "Home");
      }

      [HttpGet("/items/{id}/update")]
      public ActionResult UpdateForm(int id)
      {
        Item thisItem = Item.Find(id);
        return View(thisItem);
      }

      [HttpGet("/items/new")]
      public ActionResult CreateForm()
      {
        return View();
      }

      [HttpPost("/items/new")]
      public ActionResult Create()
      {
        string formDate = Request.Form["due-date"];
        DateTime newDate = DateTime.Parse(formDate);
        Item newItem = new Item(Request.Form["item-description"], newDate);
        newItem.Save();
        return RedirectToAction("Success", "Home");
      }
    }
}

//
// [HttpPost("/items/{id}/update")]
// public ActionResult Update(int id)
// {
//   Item thisItem = Item.Find(id);
//   thisItem.Edit(Request.Form["newname"]);
//   return RedirectToAction("index", "categories");
// }
//
// [HttpGet("/items/{id}/categories/{catId}/delete")]
// public ActionResult DeleteItemCategory(int id, int catId)
// {
//   Item thisItem = Item.Find(id);
//   thisItem.DeleteItemCategory(catId);
//
//   // return RedirectToRoute("/category/{id}", new {id = catId});
//   return RedirectToAction("CategoryDetails", "Categories", new {id = catId});
// }
//
// [HttpPost("/items/{id}/delete_all")]
// public ActionResult DeleteAll(int id)
// {
//   Item.DeleteAllByCategory(id);
//   return RedirectToAction("index", "categories");
// }

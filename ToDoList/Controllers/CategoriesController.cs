using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;
namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
      //LIST ALL CATEGOROES
      [HttpGet("/categories")]
      public ActionResult Index()
      {
        List<Category> allCategories = Category.GetAll();
        return View(allCategories);
      }

        //ADD TASK TO CATEGORY
        [HttpPost("/categories/{categoryId}/items/new")]
        public ActionResult AddItem(int categoryId)
        {
          Category category = Category.Find(categoryId);
          Item item = Item.Find(Convert.ToInt32(Request.Form["item-id"]));
          category.AddItem(item);
          return RedirectToAction("Success", "Home");
        }

        //DISPLAY CATEGORY DETAILS
        [HttpGet("/categories/{id}")]
        public ActionResult Details(int id)
        {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Category selectedCategory = Category.Find(id);
          List<Item> categoryItems = selectedCategory.GetItems();
          List<Item> allItems = Item.GetAll();
          model.Add("category", selectedCategory);
          model.Add("categoryItems", categoryItems);
          model.Add("allItems", allItems);
          return View(model);
        }

        //CREATE NEW CATEGORY
        [HttpGet("/categories/new")]
        public ActionResult CreateForm()
        {
          return View();
        }
        
        //SAVE NEWLY CREATED CATEGORY TO DB
        [HttpPost("/categories/new")]
        public ActionResult Create()
        {
          Category newCategory = new Category(Request.Form["category-name"]);
          newCategory.Save();
          return RedirectToAction("Success", "Home");
        }
    }
}

// previous actions saved for repurposing
//
// [HttpPost("/category/{id}/delete_category")]
// public ActionResult CategoryDelete(int id)
// {
//   Item.DeleteAllByCategory(id);
//   Category newCategory = Category.Find(id);
//   newCategory.DeleteCategory();
//   List<Category> allCategories = Category.GetAll();
//   return View("index", allCategories);
// }
//
// [HttpGet("/category/{id}/items/new")]
// public ActionResult CategoryAddItems(int id)
// {
//
//   return RedirectToAction("CreateForm","items", id);
// }
//
// [HttpPost("/category/create")]
// public ActionResult CreateCategory()
// {
//   Category newCategory = new Category (Request.Form["new-category"]);
//   newCategory.Save();
//   List<Category> allCategories = Category.GetAll();
//   return RedirectToAction("Index", allCategories);
// }

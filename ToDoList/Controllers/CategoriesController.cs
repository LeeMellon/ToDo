using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;
namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/index")]
        public ActionResult Index()
        {
          List<Category> allCategories = Category.GetAll();
          return View(allCategories);
        }

        [HttpGet("/category/new")]
        public ActionResult CategoryCreater()
        {
            return View();
        }

        [HttpPost("/category/create")]
        public ActionResult CreateCategory()
        {
          Category newCategory = new Category (Request.Form["new-category"]);
          newCategory.Save();
          List<Category> allCategories = Category.GetAll();
          return RedirectToAction("Index", allCategories);
        }

        [HttpGet("/category/{id}/items/new")]
        public ActionResult CategoryAddItems(int id)
        {

          return RedirectToAction("CreateForm","items", id);
        }

        [HttpGet("/category/{id}")]
        public ActionResult CategoryDetails(int id)
        {
          Category thisCategory = Category.Find(id);
          List<Item> categoryItems = Item.ItemsByCategory(id);
          Dictionary<string, object> CategoryItemDict = new Dictionary <string, object>();
          CategoryItemDict.Add("categoryName", thisCategory);
          CategoryItemDict.Add("categoryItems", categoryItems);
          return View(CategoryItemDict);
        }

        [HttpPost("/category/{id}/delete_category")]
        public ActionResult CategoryDelete(int id)
        {
          Item.DeleteAll(id);
          Category.DeleteCategory(id);
          List<Category> allCategories = Category.GetAll();
          return View("index", allCategories);
        }
    }
}

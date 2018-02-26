using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Tests
{
[TestClass]
public class ItemTest : IDisposable
{
public void Dispose()
{
        Item.DeleteAll();
        Category.DeleteAll();
}
public void ItemTests()
{
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
}

[TestMethod]
public void GetAll_DatabaseEmptyAtFirst_0()
{
        //Arrange, Act
        int result = Item.GetAll().Count;

        //Assert
        Assert.AreEqual(0, result);
}

[TestMethod]
public void GetDescription_ReturnsDescription_String()
{
        //Arrange
        string description = "Walk the dog.";
        DateTime newDate = new DateTime (2018, 1, 1);
        Item newItem = new Item(description, newDate);
        //Act
        string result = newItem.GetDescription();

        //Assert
        Assert.AreEqual(description, result);
}

[TestMethod]
public void Save_DatabaseAssignsIdToItem_Id()
{
        //Arrange
        DateTime newDate = new DateTime (2018, 1, 1);
        Item testItem = new Item("Mow the lawn",newDate);
        testItem.Save();

        //Act
        Item savedItem = Item.GetAll()[0];

        int result = savedItem.GetId();
        int testId = testItem.GetId();

        //Assert
        Assert.AreEqual(testId, result);
}



[TestMethod]
public void GetAll_ReturnsItems_Int()
{
        //Arrange
        DateTime newDate01 = new DateTime (2018, 1, 3);
        DateTime newDate02 = new DateTime (2018, 1, 2);
        string description01 = "Walk the dog";
        string description02 = "Wash the dishes";
        Item newItem1 = new Item(description01, newDate01);
        Item newItem2 = new Item(description02, newDate02);
        List<Item> newList = new List<Item> {
                newItem1, newItem2
        };
        newItem1.Save();
        newItem2.Save();
        //Act
        List<Item> result = Item.GetAll();
        int listNum = newList.Count;
        int resultNum = result.Count;
        foreach (Item thisItem in result)
        {
                Console.WriteLine("Description: " + thisItem.GetDescription());
                Console.WriteLine("Date " + thisItem.GetDueDate());
        }

        //Assert
        Assert.AreEqual(listNum, resultNum);
}

[TestMethod]
public void Save_SavesToDatabase_ItemList()
{
        //Arrange
        DateTime newDate03 = new DateTime (2018, 1, 4);
        Item testItem = new Item("Mow the lawn", newDate03);

        //Act
        testItem.Save();
        List<Item> result = Item.GetAll();
        List<Item> testList = new List<Item>{
                testItem
        };

        //Assert
        CollectionAssert.AreEqual(testList, result);
}

[TestMethod]
public void Save_AssignsIdToItem_Id()
{
        //Arrange
        DateTime newDate04 = new DateTime (2018, 1, 5);
        Item testItem = new Item("Mow the lawn", newDate04);

        //Act
        testItem.Save();
        Item savedItem = Item.GetAll()[0];

        int result = savedItem.GetId();
        int testId = testItem.GetId();

        //Assert
        Assert.AreEqual(testId, result);
}

[TestMethod]
public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
{
        // Arrange, Act
        DateTime newDate05 = new DateTime (2018, 1, 6);
        Item firstItem = new Item("Mow the lawn",newDate05);
        Item secondItem = new Item("Mow the lawn", newDate05);

        // Assert
        Assert.AreEqual(firstItem, secondItem);
}

[TestMethod]
public void Find_FindsItemInDatabase_Item()
{
        //Arrange
        DateTime newDate06 = new DateTime (2018, 1, 7);
        Item testItem = new Item("Mow the lawn", newDate06);
        testItem.Save();

        //Act
        Item foundItem = Item.Find(testItem.GetId());

        //Assert
        Assert.AreEqual(testItem, foundItem);
}

[TestMethod]
public void AddCategory_AddsCategoryToItem_CategoryList()
{
        //Arrange
        DateTime newDate06 = new DateTime (2018, 1, 7);
        Item testItem = new Item("Mow the lawn", newDate06);
        testItem.Save();

        Category testCategory = new Category("Home stuff");
        testCategory.Save();

        //Act
        testItem.AddCategory(testCategory);

        List<Category> result = testItem.GetCategories();
        List<Category> testList = new List<Category>{
                testCategory
        };

        //Assert
        CollectionAssert.AreEqual(testList, result);
}
[TestMethod]
public void GetCategories_ReturnsAllItemCategories_CategoryList()
{
        //Arrange
        DateTime newDate = new DateTime (2018, 1, 1);
        Item testItem = new Item("Mow the lawn", newDate);
        testItem.Save();

        Category testCategory1 = new Category("Home stuff");
        testCategory1.Save();

        Category testCategory2 = new Category("Work stuff");
        testCategory2.Save();

        //Act
        testItem.AddCategory(testCategory1);
        List<Category> result = testItem.GetCategories();
        List<Category> testList = new List<Category> {
                testCategory1
        };

        //Assert
        CollectionAssert.AreEqual(testList, result);
}

[TestMethod]
public void Delete_DeletesItemAssociationsFromDatabase_ItemList()
{
        //Arrange
        Category testCategory = new Category("Home stuff");
        testCategory.Save();

        DateTime newDate1 = new DateTime (2018, 1, 2);
        string testDescription = "Mow the lawn";
        Item testItem = new Item(testDescription, newDate1);
        testItem.Save();

        //Act
        testItem.AddCategory(testCategory);
        testItem.DeleteItem();

        List<Item> resultCategoryItems = testCategory.GetItems();
        List<Item> testCategoryItems = new List<Item> {
        };

        //Assert
        CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
}
}
}

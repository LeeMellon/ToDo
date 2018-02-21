[TestMethod]
    public void GetItems_RetrievesAllItemsWithCategory_ItemList()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Item firstItem = new Item("Mow the lawn", testCategory.GetId());
      firstItem.Save();
      Item secondItem = new Item("Do the dishes", testCategory.GetId());
      secondItem.Save();


      List<Item> testItemList = new List<Item> {firstItem, secondItem};
      List<Item> resultItemList = testCategory.GetItems();

      CollectionAssert.AreEqual(testItemList, resultItemList);
    }

using System.Collections.Generic;
using System;
using ToDoList;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
public class Item
{
private int _id;
private string _description;
private DateTime _dueDate;


public Item(string description, DateTime dueDate, int Id = 0)
{
        _id = Id;
        _description = description;
        _dueDate = dueDate;

}

public override bool Equals(System.Object otherItem)
{
        if (!(otherItem is Item))
        {
                return false;
        }
        else
        {
                Item newItem = (Item) otherItem;
                bool dateEquality = (this.GetDueDate() == newItem.GetDueDate());
                bool idEquality = (this.GetId() == newItem.GetId());
                bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
                return (idEquality && descriptionEquality && dateEquality);
        }
}

public override int GetHashCode()
{
        return this.GetId().GetHashCode();
}

public int GetId()
{
        return _id;
}
public void SetId(int newId)
{
        _id = newId;
}
public string GetDescription()
{
        return _description;
}
public void SetDescription(string newDescription)
{
        _description = newDescription;
}

public string GetDueDate()
{
        string dateToString = _dueDate.ToString("MM/dd/yyyy");
        return dateToString;
}

public void SetDueDate(DateTime newDueDate)
{
        _dueDate = newDueDate;
}

public static List<Item> GetAll()
{
        List<Item> allItems = new List<Item> {
        };
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM items;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                DateTime itemDate = rdr.GetDateTime(2);
                Item newItem = new Item(itemDescription, itemDate, itemId);
                allItems.Add(newItem);
        }
        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }
        return allItems;
}

public static void DeleteAllByCategory(int id)
{
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items WHERE category_id = @id;";

        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName= "@id";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        conn.Close();

        if (conn != null)
        {
                conn.Dispose();
        }
}

public static void DeleteAll()
{
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }
}

public void DeleteItem()
{
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @ItemId; DELETE FROM categories_items WHERE item_id = @ItemId;";

      MySqlParameter itemIdParameter = new MySqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = this.GetId();;
      cmd.Parameters.Add(itemIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

public void Save()
{
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO items (description, duedate) VALUES (@ItemDescription, @ItemDueDate);";

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@ItemDescription";
        description.Value = this._description;
        cmd.Parameters.Add(description);

        MySqlParameter due_date = new MySqlParameter();
        due_date.ParameterName = "@ItemDueDate";
        due_date.Value = this._dueDate;
        cmd.Parameters.Add(due_date);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }
}

public static Item Find(int id)
{
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM items WHERE id = @thisId;";

        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int itemId = 0;
        string itemDescription = "";
        DateTime itemDueDate = new DateTime (2018, 1, 1);

        while (rdr.Read())
        {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
                itemDueDate = rdr.GetDateTime(2);
        }

        Item foundItem= new Item(itemDescription, itemDueDate, itemId);

        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }

        return foundItem;
}

public void Edit(string newDescription)
{
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@newDescription";
        description.Value = newDescription;
        cmd.Parameters.Add(description);

        cmd.ExecuteNonQuery();
        _description = newDescription;

        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }
}

public void AddCategory(Category newCategory)
{
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

        MySqlParameter category_id = new MySqlParameter();
        category_id.ParameterName = "@CategoryId";
        category_id.Value = newCategory.GetId();
        cmd.Parameters.Add(category_id);

        MySqlParameter item_id = new MySqlParameter();
        item_id.ParameterName = "@ItemId";
        item_id.Value = _id;
        cmd.Parameters.Add(item_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
                conn.Dispose();
        }
}
public List<Category> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT categories.* FROM items JOIN categories_items ON (items.id = categories_items.item_id)
            JOIN categories ON (categories_items.category_id = categories.id)
            WHERE items.id = @itemId;";

            MySqlParameter itemIdParameter = new MySqlParameter();
            itemIdParameter.ParameterName = "@itemId";
            itemIdParameter.Value = _id;
            cmd.Parameters.Add(itemIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
              List<Category> categories = new List<Category>{};

              while(rdr.Read())
              {
                int categoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category newCategory = new Category(categoryName, categoryId);
                categories.Add(newCategory);
              }
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
              return categories;
          }

}
}

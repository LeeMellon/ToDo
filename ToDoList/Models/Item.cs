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
      List<Item> allItems = new List<Item> {};
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

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"INSERT INTO items (description, dueDate) VALUES (@ItemDescription, @ItemDueDate);";

     MySqlParameter description = new MySqlParameter();
     description.ParameterName = "@ItemDescription";
     description.Value = this._description;
     cmd.Parameters.Add(description);

     MySqlParameter dueDate = new MySqlParameter(); 
     dueDate.ParameterName = "@ItemDueDate";
     dueDate.Value = this._dueDate;
     cmd.Parameters.Add(dueDate);

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

    public static List<Item> DateDesc()
      {
          List<Item> allItems = new List<Item> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM items ORDER BY duedate ASC;" ;
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
  }
}

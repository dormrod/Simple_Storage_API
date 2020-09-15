using System;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;
using Storage.API.Models;

namespace Storage.API.Services
{
    public class StorageRepository
    {

        SQLiteConnection connection;

        public StorageRepository()
        {

            //Set database information
            string pwd = Directory.GetCurrentDirectory();
            string dbPath = string.Format("{0}/Database/storage.db", pwd);
            connection = new SQLiteConnection(string.Format("Data Source={0};", dbPath));

            //Make new database if does not exist
            connection.Open();   
	        var command = new SQLiteCommand(connection);
            command.CommandText = "SELECT name FROM sqlite_master WHERE name = 'storage';";
            var name = command.ExecuteScalar();
            if (name == null) ExecuteSQLiteNonQuery("CREATE TABLE storage(id INTEGER PRIMARY KEY, quantity INTEGER, name STRING, location STRING);");
            connection.Close();
        }

        public string Reset()
        {
            //Delete all database contents
            connection.Open();
            ExecuteSQLiteNonQuery("DROP TABLE IF EXISTS storage;");
            ExecuteSQLiteNonQuery("CREATE TABLE storage(id INTEGER PRIMARY KEY, quantity INTEGER, name STRING, location STRING);");
            connection.Close();
			return "Database reset";
		}

        public void AddItem(int quantity, string name, string location)
        {
            var item = new StorageItem();
            item.name = name;
            item.quantity = quantity;
            item.location = location;
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO storage(quantity,name,location) VALUES(@quantity,@name,@location)";
            command.Parameters.AddWithValue("@quantity", item.quantity);
            command.Parameters.AddWithValue("@name", item.name);
            command.Parameters.AddWithValue("@location", item.location);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<StorageItem> GetAllItems()
        {
            string query = "SELECT * FROM storage;";
            connection.Open();
            var items = ExecuteSQLiteReader(query);
            connection.Close();
            return items;
        }

        public List<StorageItem> GetQueryItems(string name, string location)
        {
            string query = "SELECT * FROM storage";
			if(name=="" && location=="")
            {
                query += ";";
			}    
            else if (name=="")
            {
                query += string.Format(" WHERE location='{0}';", location);
			}
            else if (location=="")
            {
                query += string.Format(" WHERE name='{0}';", name);
			}
            else
            {
                query += string.Format(" WHERE name='{0}' AND location='{1}';", name, location);
            }

			connection.Open();
            var items = ExecuteSQLiteReader(query);
            connection.Close();
            return items;
        }

        private void ExecuteSQLiteNonQuery(string commandText)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        private List<StorageItem> ExecuteSQLiteReader(string commandText)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = commandText;
            Console.WriteLine(commandText);
            SQLiteDataReader reader = command.ExecuteReader();

            var items = new List<StorageItem>();
            while (reader.Read())
            {
                StorageItem item = new StorageItem();
                item.id = reader.GetInt32(0);
                item.quantity = reader.GetInt32(1);
                item.name = reader.GetString(2);
                item.location = reader.GetString(3);
                items.Add(item);
            }

            return items;
        }
    }
}

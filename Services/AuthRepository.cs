using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.AspNetCore.Mvc;
using Storage.API.Models;
using System.Text;

namespace Storage.API.Services
{
    public class AuthRepository
    {
        SQLiteConnection connection;

        public AuthRepository()
        {

            //Set database information
            string pwd = Directory.GetCurrentDirectory();
            string dbPath = string.Format("{0}/Database/users.db", pwd);
            connection = new SQLiteConnection(string.Format("Data Source={0};", dbPath));

            //Make new database if does not exist
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "SELECT name FROM sqlite_master WHERE name = 'users';";
            var name = command.ExecuteScalar();
            if (name == null)
            {
                ExecuteSQLiteNonQuery("CREATE TABLE users(id INTEGER PRIMARY KEY, username STRING, passwordHash STRING, passwordSalt STRING);");
                ExecuteSQLiteNonQuery("CREATE UNIQUE INDEX username on users(username);");
            }
            connection.Close();
        }

        private void ExecuteSQLiteNonQuery(string commandText)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        public bool RegisterUser(string username, string password)
        {

            var user = new User();
            user.username = username;
            user.SetPassword(password);
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO users(username,passwordHash,passwordSalt) VALUES(@username,@passwordHash,@passwordSalt)";
            command.Parameters.AddWithValue("@username", user.username);
            command.Parameters.AddWithValue("@passwordHash", user.passwordHash);
            command.Parameters.AddWithValue("@passwordSalt", user.passwordSalt);
            bool error;
            try
            {
                command.ExecuteNonQuery();
                error = false;
            }
            catch (SQLiteException)
            {
                error = true;
            }
            connection.Close();
            return error;
        }

        public List<User> GetUser(string username)
        {
            string query = string.Format("SELECT * FROM users WHERE username='{0}';", username);
            connection.Open();
            var users = ExecuteSQLiteReader(query);
            connection.Close();
            return users;
        }

        public (bool authorised, string errorMessage) AuthoriseUser(string username, string password)
        {
            var user = GetUser(username);
            if (user.Count == 0) return (false, "Username incorrect.");
            var authorised = user[0].VerifyPassword(password);
            if (!authorised) return (false,"Password incorrect.");
            return (true, "");
        }

        private List<User> ExecuteSQLiteReader(string commandText)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = commandText;
            Console.WriteLine(commandText);
            SQLiteDataReader reader = command.ExecuteReader();

            var users = new List<User>();
            while (reader.Read())
            {
                var user = new User();
                user.id = reader.GetInt32(0);
                user.username = reader.GetString(1);
                user.passwordHash = reader.GetString(2);
                user.passwordSalt = reader.GetString(3);
                users.Add(user);
            }
            return users;
        }
    }
}

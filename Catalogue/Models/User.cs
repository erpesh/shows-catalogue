using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public static void RegisterUser(string username, string password)
        {
            var users = LoadUsers();
            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("User already exists. Please choose a different username.");
            }
            else
            {
                string hashedPassword = PasswordHasher.HashPassword(password);
                users.Add(new User { Username = username, HashedPassword = hashedPassword });
                SaveUsers(users);
                Console.WriteLine("User registered successfully.");
            }
        }

        public static bool Login(string username, string password)
        {
            var users = LoadUsers();
            var user = users.Find(u => u.Username == username);
            if (user != null && PasswordHasher.VerifyPassword(user.HashedPassword, password))
            {
                File.WriteAllText("currentuser.txt", username); // Store current user in a file
                Console.WriteLine("Login successful.");
                return true;
            }
            else
            {
                Console.WriteLine("Login failed. Invalid username or password.");
                return false;
            }
        }

        public static string GetCurrentLoggedInUser()
        {
            if (File.Exists("currentuser.txt"))
            {
                return File.ReadAllText("currentuser.txt");
            }
            return null;
        }

        public static List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                try
                {
                    users = JsonSerializer.Deserialize<List<User>>(json);
                }
                catch (Exception ex)
                {

                }
                
            }
            return users;
        }

        private static void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText("users.json", json);
        }
    }
}

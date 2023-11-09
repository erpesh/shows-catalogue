using Catalogue.Models;

namespace Catalogue
{
    class Program
    {
        static void Main()
        {
            string currentUser = User.GetCurrentLoggedInUser();

            if (currentUser != null)
            {
                Console.WriteLine($"Welcome, {currentUser}!");
            }
            else
            {
                Console.WriteLine("No user is currently logged in.");
            }

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter your username: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter your password: ");
                        string password = Console.ReadLine();

                        User.RegisterUser(username, password);
                        break;

                    case "2":
                        Console.Write("Enter your username: ");
                        string loginUsername = Console.ReadLine();
                        Console.Write("Enter your password: ");
                        string loginPassword = Console.ReadLine();

                        User.Login(loginUsername, loginPassword);
                        break;

                    case "3":
                        Environment.Exit(0);
                        break;

                    case "4":
                        DisplayAllUsers();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void DisplayAllUsers()
        {
            var users = User.LoadUsers();

            if (users.Count > 0)
            {
                Console.WriteLine("Registered Users:");
                foreach (var user in users)
                {
                    Console.WriteLine(user.Username);
                }
            }
            else
            {
                Console.WriteLine("No registered users yet.");
            }
        }
    }
}
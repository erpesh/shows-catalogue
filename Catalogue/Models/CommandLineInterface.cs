using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class CommandLineInterface
    {
        private readonly string[] commands = { 
            "help", 
            "register",
            "login",
            "logout",
            "addfilm",
            "addseries",
            "addactor",
            "search",
        };

        public void ExecuteCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Type 'Catalogue help' to see available commands.");
                return;
            }

            string command = args[0].ToLower();
            
            switch (command)
            {
                case "help":
                    break;
                case "register":
                    break;
                case "login":
                    break;
                case "logout":
                    break;
                case "addfilm":
                    break;
                case "addseries":
                    break;
                case "addactor":
                    break;
                case "search":
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine("Invalid command. Available commands: addfilm, addseries, addactor, listfilms, listseries, listactors");
                    break;
            }
        }

        private Person InputPerson()
        {
            while (true)
            {

            }
        }

        private void AddFilm(string[] args)
        {
            while (true)
            {
                Console.Write("Enter the title: ");
                string title = Console.ReadLine().Trim();

                Console.Write("Enter the description: ");
                string description = Console.ReadLine().Trim();

                Console.Write("Enter genres (comma-separated): ");
                List<string> genres = Console.ReadLine().Split(',').ToList();

                Console.Write("Enter studio: ");
                string studio = Console.ReadLine().Trim();

                Console.Write("Enter the director's name: ");
                string directorName = Console.ReadLine().Trim();

                Console.Write("Enter the episode length (in minutes): ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int episodeLength) || episodeLength <= 0)
                {
                    Console.WriteLine("Invalid episode length. It must be a positive integer.");
                    continue;
                }

                Console.Write("Enter the release date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine().Trim(), out DateTime releaseDate))
                {
                    Console.WriteLine("Invalid release date. It must be a valid date in the format YYYY-MM-DD.");
                    continue;
                }

                // Find the director in the existing list or create a new one
                Person director = new Person(directorName);

                // Create the film object
                Film newFilm = new Film(1, title, description, genres, studio, director, new List<Actor>(), episodeLength, DateOnly.FromDateTime(releaseDate));

                // Print a success message
                Console.WriteLine($"Film '{title}' added successfully!");
                break; // Exit the loop if everything is successful
            }
        }
    }
}

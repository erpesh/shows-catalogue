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
                    //InputFilm();
                    break;
                case "addseries":
                    break;
                case "addactor":
                    break;
                case "addperson":
                    var person = InputPerson();
                    DataStorage.SaveStaffMember(person);
                    break;
                case "search":
                    var members = DataStorage.LoadStaff();
                    foreach (var member in members)
                    {
                        Console.WriteLine($"{member.FirstName} {member.LastName}");
                    }
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
                Console.WriteLine("Enter person details:");

                Console.Write("First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Date of Birth (YYYY-MM-DD): ");
                DateOnly dateOfBirth = DateOnly.Parse(Console.ReadLine());

                Console.Write("Nationality: ");
                string nationality = Console.ReadLine();

                Person newPerson = new Person(firstName, lastName, dateOfBirth, nationality);

                return newPerson;
            }
        }

        //private Film InputFilm()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("Enter film details:");

        //        Console.Write("Title: ");
        //        string title = Console.ReadLine();

        //        Console.Write("Description: ");
        //        string description = Console.ReadLine();

        //        Console.Write("Genre: ");
        //        string genre = Console.ReadLine();

        //        Console.Write("Studio: ");
        //        string studio = Console.ReadLine();

        //        // Input director details using the InputPerson function
        //        Person director = InputPerson();

        //        // Input actors details using the InputPerson function for each actor
        //        List<Actor> actors = new List<Actor>();
        //        Console.Write("Enter the number of actors: ");
        //        int numberOfActors = int.Parse(Console.ReadLine());

        //        for (int i = 0; i < numberOfActors; i++)
        //        {
        //            Console.WriteLine($"Enter details for Actor {i + 1}:");
        //            actors.Add(InputPerson());
        //        }

        //        Console.Write("Episode Length (in minutes, if applicable): ");
        //        int? episodeLength = int.TryParse(Console.ReadLine(), out int length) ? length : (int?)null;

        //        Console.Write("Release Date (YYYY-MM-DD): ");
        //        DateOnly? releaseDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly date) ? date : (DateOnly?)null;

        //        // Creating and returning a new Film object
        //        Film newFilm = new Film(title, description, new List<string> { genre }, studio, director, actors, episodeLength, releaseDate);

        //        return newFilm;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class CommandLineInterface
    {
        private const string appName = "Catalogue";
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
                Console.WriteLine($"Type '{appName} help' to see available commands.");
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
                    var film = InputFilm();
                    DataStorage.SaveFilm(film);
                    break;
                case "addseries":
                    var series = InputSeries();
                    DataStorage.SaveSeries(series);
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
                        Console.WriteLine($"{member.Id} - {member.FirstName} {member.LastName}");
                    }
                    break;
                case "review":
                    if (args.Length < 4)
                    {
                        Console.WriteLine("Please specify a subcommand (add, delete), type (film, series), and ID.");
                        return;
                    }

                    string[] subcommands = { "add", "delete" };
                    string[] types = { "film", "series" };

                    string subcommand = args[1].ToLower();
                    string type = args[2].ToLower();
                    int id = int.Parse(args[3]);

                    if (!subcommands.Contains(subcommand))
                    {
                        Console.WriteLine("Invalid subcommand. Available subcommands: add, delete");
                        return;
                    }
                    if (!types.Contains(type))
                    {
                        Console.WriteLine("Invalid type. Available types: film, series");
                        return;
                    }
                    

                    if (subcommand == "add")
                    {
                        if (type == "film")
                        {
                            var show = DataStorage.LoadFilm(id);
                            var review = InputReview();
                            show.AddReview(review);
                            DataStorage.UpdateFilm(show);
                        }
                        else if (type == "series")
                        {
                            var show = DataStorage.LoadSeries(id);
                            var review = InputReview();
                            show.AddReview(review);
                            DataStorage.UpdateSeries(show);
                        }
                        Console.WriteLine("Review added.");
                    }
                    else if (subcommand == "delete")
                    {
                        if (type == "film")
                        {
                            var show = DataStorage.LoadFilm(id);
                            show.DeleteReview("erp");
                            DataStorage.UpdateFilm(show);
                        }
                        else if (type == "series")
                        {
                            var show = DataStorage.LoadSeries(id);
                            show.DeleteReview("erp");
                            DataStorage.UpdateSeries(show);
                        }
                        Console.WriteLine("Review deleted.");
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

        private Film InputFilm()
        {
            Console.WriteLine("Enter film details:");

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Genres (comma-separated): ");
            List<string> genres = Console.ReadLine().Split(',').Select(g => g.Trim()).ToList();

            Console.Write("Studio: ");
            string studio = Console.ReadLine();

            Console.Write("Director: ");
            string director = Console.ReadLine();

            Console.Write("Actors (comma-separated): ");
            List<string> actors = Console.ReadLine().Split(',').Select(a => a.Trim()).ToList();

            Console.Write("Episode Length (in minutes, optional): ");
            int? episodeLength = int.TryParse(Console.ReadLine(), out int length) ? length : (int?)null;

            Console.Write("Release Date (YYYY-MM-DD, optional): ");
            DateOnly? releaseDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly rDate) ? rDate : (DateOnly?)null;

            return new Film(title, description, genres, studio, director, actors, episodeLength, releaseDate);
        }

        private Series InputSeries()
        {
            Console.WriteLine("Enter series details:");

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Genres (comma-separated): ");
            List<string> genres = Console.ReadLine().Split(',').Select(g => g.Trim()).ToList();

            Console.Write("Studio: ");
            string studio = Console.ReadLine();

            Console.Write("Director: ");
            string director = Console.ReadLine();

            Console.Write("Actors (comma-separated): ");
            List<string> actors = Console.ReadLine().Split(',').Select(a => a.Trim()).ToList();

            Console.Write("Episode Length (in minutes, optional): ");
            int? episodeLength = int.TryParse(Console.ReadLine(), out int length) ? length : (int?)null;

            Console.Write("Seasons (optional): ");
            int? seasons = int.TryParse(Console.ReadLine(), out int s) ? s : (int?)null;

            Console.Write("Episodes per Season (optional): ");
            int? episodesPerSeason = int.TryParse(Console.ReadLine(), out int eps) ? eps : (int?)null;

            Console.Write("Start Date (YYYY-MM-DD, optional): ");
            DateOnly? startDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly sDate) ? sDate : (DateOnly?)null;

            Console.Write("End Date (YYYY-MM-DD, optional): ");
            DateOnly? endDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly eDate) ? eDate : (DateOnly?)null;

            return new Series(title, description, genres, studio, director, actors, episodeLength, seasons, episodesPerSeason, startDate, endDate);
        }
        private Review InputReview()
        {
            Console.WriteLine("Enter review details:");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Rating (1-10): ");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 10)
            {
                throw new FormatException("Invalid rating. Please enter a valid integer between 1 and 10.");
            }

            Console.Write("Comment (optional): ");
            string? comment = Console.ReadLine();
            comment = string.IsNullOrWhiteSpace(comment) ? null : comment;

            return new Review(username, rating, comment);
        }
    }
}

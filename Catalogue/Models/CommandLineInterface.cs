using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class MediaTypeException : Exception
    {
        public MediaTypeException(string[] validTypes)
            : base($"Please specify a valid type ({string.Join(", ", validTypes)}).")
        {
        }
    }
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message) : base(message)
        {
        }
    }
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message)
        {
        }
    }
    class CommandLineInterface
    {
        private const string appName = "Catalogue";
        private readonly string[] commands = { 
            "help", 
            "login",
            "logout",
            "addfilm",
            "addseries",
            "addactor",
            "search",
            "review"
        };
        private readonly string[] types = { "film", "series", "actor" };

        public void ExecuteCommand(string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidCommandException($"Type '{appName} help' to see available commands.");
            }

            string command = args[0].ToLower();
            string type;
            int id;
            string username;

            try
            {
                switch (command)
                {
                    case "help":
                        Console.WriteLine("Available commands:");
                        foreach (var cmd in commands)
                        {
                            switch (cmd)
                            {
                                case "help":
                                    Console.WriteLine($"{appName} {cmd} - Display available commands and their usage.");
                                    break;
                                case "login":
                                    Console.WriteLine($"{appName} {cmd} - Log in to the application.");
                                    break;
                                case "logout":
                                    Console.WriteLine($"{appName} {cmd} - Log out from the application.");
                                    break;
                                case "addfilm":
                                    Console.WriteLine($"{appName} {cmd} - Add a new film to the catalogue.");
                                    break;
                                case "addseries":
                                    Console.WriteLine($"{appName} {cmd} - Add a new series to the catalogue.");
                                    break;
                                case "addactor":
                                    Console.WriteLine($"{appName} {cmd} - Add a new actor to the catalogue.");
                                    break;
                                case "addperson":
                                    Console.WriteLine($"{appName} {cmd} - Add a new person to the catalogue.");
                                    break;
                                case "search":
                                    Console.WriteLine($"{appName} {cmd} - Search and display staff members.");
                                    break;
                                case "review":
                                    Console.WriteLine($"{appName} {cmd} - Manage reviews for films and series.");
                                    Console.WriteLine($"  {appName} {cmd} add <type> <id> - Add a review for a show (film or series) with the specified ID.");
                                    Console.WriteLine($"  {appName} {cmd} delete <type> <id> - Delete a review for a show (film or series) with the specified ID.");
                                    break;
                                default:
                                    Console.WriteLine($"Invalid command: {cmd}");
                                    break;
                            }
                        }
                        break;
                    case "login":
                        username = args.Length > 1 ? args[1].Trim() : null;
                        if (string.IsNullOrWhiteSpace(username))
                        {
                            throw new ArgumentException("Specify your username.");
                        }

                        DataStorage.SaveUsername(username);
                        Console.WriteLine($"Logged in as {username}.");
                        break;
                    case "logout":
                        DataStorage.RemoveUsername();
                        Console.WriteLine("Logged out.");
                        break;
                    case "add":
                        if (args.Length < 2)
                        {
                            throw new MediaTypeException(types);
                        }

                        type = args[1].ToLower();
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                            var show = InputFilm();
                            DataStorage.SaveFilm(show);
                        }
                        else if (type == "series")
                        {
                            var show = InputSeries();
                            DataStorage.SaveSeries(show);
                        }
                        else if (type == "actor")
                        {
                            var person = InputPerson();
                            DataStorage.SaveActor(person);
                        }

                        Console.WriteLine("Show added.");
                        break;
                    case "edit":
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException("Please specify a type (film, series) and ID.");
                        }

                        type = args[1].ToLower();
                        id = int.Parse(args[2]);

                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                        }
                        else if (type == "series")
                        {
                        }
                        else if (type == "actor")
                        {

                        }

                        Console.WriteLine("Show edited.");
                        break;
                    case "delete":
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException("Please specify a type (film, series, ) and ID.");
                        }

                        type = args[1].ToLower();
                        id = int.Parse(args[2]);

                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                            DataStorage.DeleteFilm(id);
                        }
                        else if (type == "series")
                        {
                            DataStorage.DeleteSeries(id);
                        }

                        Console.WriteLine("Show deleted.");
                        break;
                    case "review":
                        if (args.Length < 4)
                        {
                            throw new InvalidCommandException("Please specify a subcommand (add, delete), type (film, series), and ID.");
                        }

                        string[] reviewTypes = { "film", "series" };
                        string[] subcommands = { "add", "delete" };

                        string subcommand = args[1].ToLower();
                        type = args[2].ToLower();
                        id = int.Parse(args[3]);

                        if (!subcommands.Contains(subcommand))
                        {
                            throw new InvalidCommandException("Invalid subcommand. Available subcommands: add, delete");
                        }
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(reviewTypes);
                        }

                        username = DataStorage.LoadUsername();
                        if (string.IsNullOrWhiteSpace(username))
                        {
                            throw new UnauthorizedAccessException("Please log in to add a review.");
                        }

                        if (subcommand == "add")
                        {
                            if (type == "film")
                            {
                                var show = DataStorage.LoadFilm(id);
                                var review = InputReview(username);
                                show.AddReview(review);
                                DataStorage.UpdateFilm(show);
                            }
                            else if (type == "series")
                            {
                                var show = DataStorage.LoadSeries(id);
                                var review = InputReview(username);
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
                                show.DeleteReview(username);
                                DataStorage.UpdateFilm(show);
                            }
                            else if (type == "series")
                            {
                                var show = DataStorage.LoadSeries(id);
                                show.DeleteReview(username);
                                DataStorage.UpdateSeries(show);
                            }
                            Console.WriteLine("Review deleted.");
                        }
                        break;
                    case "search":
                        break;
                    default:
                        throw new InvalidCommandException($"Invalid command. Type '{appName} help' to see available commands.");
                }
            }
            catch (Exception error)
            {

            }
        }

        private Actor InputPerson()
        {
            while (true)
            {
                Console.WriteLine("Enter person details:");

                Console.Write("Full Name: ");
                string fullName = Console.ReadLine();

                Console.Write("Date of Birth (YYYY-MM-DD): ");
                DateOnly dateOfBirth = DateOnly.Parse(Console.ReadLine());

                Console.Write("Nationality: ");
                string nationality = Console.ReadLine();

                Console.Write("Filmography (comma-separated): ");
                List<string> filmography = Console.ReadLine().Split(',').Select(f => f.Trim()).ToList();

                Actor newPerson = new Actor(fullName, dateOfBirth, nationality, filmography);

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
        private Review InputReview(string username)
        {
            Console.WriteLine("Enter review details:");

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

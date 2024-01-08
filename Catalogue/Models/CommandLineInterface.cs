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
        private string typesString
        {
            get => string.Join(", ", types);
        }

        public void ExecuteCommand(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new InvalidCommandException($"Type '{appName} help' to see available commands.");
                }

                string command = args[0].ToLower();
                string type;
                int id;
                string username;

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
                            var person = InputActor();
                            DataStorage.SaveActor(person);
                        }

                        Console.WriteLine("Show added.");
                        break;
                    case "edit":
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
                        }

                        type = args[1].ToLower();
                        id = int.Parse(args[2]);

                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                            var film = DataStorage.LoadFilm(id);
                            EditFilm(film);
                            DataStorage.UpdateFilm(film);
                        }
                        else if (type == "series")
                        {
                            var series = DataStorage.LoadSeries(id);
                            EditSeries(series);
                            DataStorage.UpdateSeries(series);
                        }
                        else if (type == "actor")
                        {
                            var actor = DataStorage.LoadActor(id);
                            EditActor(actor);
                            DataStorage.UpdateActor(actor);
                        }
                        break;
                    case "delete":
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
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
                        else if (type == "actor")
                        {
                            DataStorage.DeleteActor(id);
                        }

                        Console.WriteLine("Show deleted.");
                        break;
                    case "review":
                        if (args.Length < 4)
                        {
                            throw new InvalidCommandException($"Please specify a subcommand (add, delete), type ({typesString}), and ID.");
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
                Console.WriteLine(error.Message);
            }
        }
        //private Film InputFilm()
        //{
        //    Console.WriteLine("Enter film details:");

        //    Console.Write("Title: ");
        //    string title = Console.ReadLine();

        //    Console.Write("Description (optional): ");
        //    string description = Console.ReadLine();

        //    Console.Write("Genres (comma-separated): ");
        //    List<string> genres = Console.ReadLine().Split(',').Select(g => g.Trim()).ToList();

        //    Console.Write("Studio: ");
        //    string studio = Console.ReadLine();

        //    Console.Write("Director: ");
        //    string director = Console.ReadLine();

        //    Console.Write("Actors (comma-separated): ");
        //    List<string> actors = Console.ReadLine().Split(',').Select(a => a.Trim()).ToList();

        //    Console.Write("Episode Length (in minutes, optional): ");
        //    int? episodeLength = int.TryParse(Console.ReadLine(), out int length) ? length : (int?)null;

        //    Console.Write("Release Date (YYYY-MM-DD, optional): ");
        //    DateOnly? releaseDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly rDate) ? rDate : (DateOnly?)null;

        //    return new Film(title, description, genres, studio, director, actors, episodeLength, releaseDate);
        //}

        //private Series InputSeries()
        //{
        //    Console.WriteLine("Enter series details:");

        //    Console.Write("Title: ");
        //    string title = Console.ReadLine();

        //    Console.Write("Description (optional): ");
        //    string description = Console.ReadLine();

        //    Console.Write("Genres (comma-separated): ");
        //    List<string> genres = Console.ReadLine().Split(',').Select(g => g.Trim()).ToList();

        //    Console.Write("Studio: ");
        //    string studio = Console.ReadLine();

        //    Console.Write("Director: ");
        //    string director = Console.ReadLine();

        //    Console.Write("Actors (comma-separated): ");
        //    List<string> actors = Console.ReadLine().Split(',').Select(a => a.Trim()).ToList();

        //    Console.Write("Episode Length (in minutes, optional): ");
        //    int? episodeLength = int.TryParse(Console.ReadLine(), out int length) ? length : (int?)null;

        //    Console.Write("Seasons (optional): ");
        //    int? seasons = int.TryParse(Console.ReadLine(), out int s) ? s : (int?)null;

        //    Console.Write("Episodes per Season (optional): ");
        //    int? episodesPerSeason = int.TryParse(Console.ReadLine(), out int eps) ? eps : (int?)null;

        //    Console.Write("Start Date (YYYY-MM-DD, optional): ");
        //    DateOnly? startDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly sDate) ? sDate : (DateOnly?)null;

        //    Console.Write("End Date (YYYY-MM-DD, optional): ");
        //    DateOnly? endDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly eDate) ? eDate : (DateOnly?)null;

        //    return new Series(title, description, genres, studio, director, actors, episodeLength, seasons, episodesPerSeason, startDate, endDate);
        //}
        private string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }
        private string? GetOptionalInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine().Trim();
            return string.IsNullOrWhiteSpace(input) ? null : input;
        }
        private List<string> GetCommaSeparated(string propmt)
        {
            return GetInput(propmt).Split(',').Select(a => a.Trim()).ToList();  
        }
        private int? GetOptionalInt(string prompt)
        {
            return int.TryParse(GetInput(prompt), out int value) ? (int?)value : null;
        }
        private DateOnly GetDate(string prompt)
        {
            return DateOnly.Parse(GetInput(prompt));
        }
        private DateOnly? GetOptionalDate(string prompt)
        {
            return DateOnly.TryParse(GetInput(prompt), out DateOnly date) ? (DateOnly?)date : null;
        }
        private T EditField<T>(string prompt, Func<string, T> inputFunction, T currentValue)
        {
            Console.Write($"{prompt} ({currentValue}): ");
            string userInput = Console.ReadLine().Trim();

            return string.IsNullOrWhiteSpace(userInput) ? currentValue : inputFunction($"{prompt}: ");
        }
        private List<string> EditListField(string prompt, List<string> currentList)
        {
            Console.Write($"{prompt} ({string.Join(", ", currentList)}): ");
            string userInput = Console.ReadLine().Trim();

            return string.IsNullOrWhiteSpace(userInput) ? currentList : userInput.Split(',').Select(a => a.Trim()).ToList();
        }
        private Series InputSeries()
        {
            Console.WriteLine("Enter series details:");

            string title = GetInput("Title: ");
            string? description = GetOptionalInput("Description (optional): ");
            List<string> genres = GetCommaSeparated("Genres (comma-separated): ");
            string studio = GetInput("Studio: ");
            string director = GetInput("Director: ");
            List<string> actors = GetCommaSeparated("Actors (comma-separated): ");
            int? episodeLength = GetOptionalInt("Episode Length (in minutes, optional): ");
            int? seasons = GetOptionalInt("Seasons (optional): ");
            int? episodesPerSeason = GetOptionalInt("Episodes per Season (optional): ");
            DateOnly? startDate = GetOptionalDate("Start Date (YYYY-MM-DD, optional): ");
            DateOnly? endDate = GetOptionalDate("End Date (YYYY-MM-DD, optional): ");

            return new Series(title, description, genres, studio, director, actors, episodeLength, seasons, episodesPerSeason, startDate, endDate);
        }
        private Film InputFilm()
        {
            Console.WriteLine("Enter series details:");

            string title = GetInput("Title: ");
            string? description = GetOptionalInput("Description (optional): ");
            List<string> genres = GetCommaSeparated("Genres (comma-separated): ");
            string studio = GetInput("Studio: ");
            string director = GetInput("Director: ");
            List<string> actors = GetCommaSeparated("Actors (comma-separated): ");
            int? episodeLength = GetOptionalInt("Film Length (in minutes, optional): ");
            DateOnly? releaseDate = GetOptionalDate("Release Date (YYYY-MM-DD, optional): ");

            return new Film(title, description, genres, studio, director, actors, episodeLength, releaseDate);
        }
        private Actor InputActor()
        { 
            Console.WriteLine("Enter actor details:");

            string fullName = GetInput("Full Name: ");
            DateOnly dateOfBirth = GetDate("Date of Birth (YYYY-MM-DD): ");
            string nationality = GetInput("Nationality: ");
            List<string> filmography = GetCommaSeparated("Filmography (comma-separated): ");

            return new Actor(fullName, dateOfBirth, nationality, filmography);
        }
        private Review InputReview(string username)
        {
            Console.WriteLine("Enter review details:");

            Console.Write("Rating (1-10): ");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 10)
            {
                throw new FormatException("Invalid rating. Please enter a valid integer between 1 and 10.");
            }

            string? comment = GetOptionalInput("Comment (optional): ");

            return new Review(username, rating, comment);
        }
        private void EditFilm(Film film)
        {
            Console.WriteLine("Editing film details. Press Enter to skip a field.");

            film.Title = EditField("Title", s => s, film.Title);
            film.Description = EditField("Description (optional)", GetOptionalInput, film.Description);
            film.Genres = EditListField("Genres (comma-separated)", film.Genres);
            film.Studio = EditField("Studio", s => s, film.Studio);
            film.Director = EditField("Director", s => s, film.Director);
            film.Actors = EditListField("Actors (comma-separated)", film.Actors);
            film.EpisodeLength = EditField("Film Length (in minutes, optional)", GetOptionalInt, film.EpisodeLength);
            film.ReleaseDate = EditField("Release Date (YYYY-MM-DD, optional)", GetOptionalDate, film.ReleaseDate);


            Console.WriteLine("Film editing complete.");
        }

        private void EditSeries(Series series)
        {
            Console.WriteLine("Editing series details. Press 'Enter' to skip a field.");

            series.Title = EditField("Title", s => s, series.Title);
            series.Description = EditField("Description (optional)", GetOptionalInput, series.Description);
            series.Genres = EditListField("Genres (comma-separated)", series.Genres);
            series.Studio = EditField("Studio", s => s, series.Studio);
            series.Director = EditField("Director", s => s, series.Director);
            series.Actors = EditListField("Actors (comma-separated)", series.Actors);
            series.EpisodeLength = EditField("Episode Length (in minutes, optional)", GetOptionalInt, series.EpisodeLength);
            series.Seasons = EditField("Seasons (optional)", GetOptionalInt, series.Seasons);
            series.Episodes = EditField("Episodes per Season (optional)", GetOptionalInt, series.Episodes);
            series.StartDate = EditField("Start Date (YYYY-MM-DD, optional)", GetOptionalDate, series.StartDate);
            series.EndDate = EditField("End Date (YYYY-MM-DD, optional)", GetOptionalDate, series.EndDate);

            Console.WriteLine("Series editing complete.");
        }

        private void EditActor(Actor actor)
        {
            Console.WriteLine("Editing actor details. Press Enter to skip a field.");

            actor.FullName = EditField("Full Name", s => s, actor.FullName);
            actor.DateOfBirth = EditField("Date of Birth (YYYY-MM-DD, optional)", GetDate, actor.DateOfBirth);
            actor.Nationality = EditField("Nationality", s => s, actor.Nationality);
            actor.Filmography = EditListField("Filmography (comma-separated)", actor.Filmography);

            Console.WriteLine("Actor editing complete.");
        }
    }
}

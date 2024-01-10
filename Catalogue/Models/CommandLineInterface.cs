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
            "stats",
            "login",
            "logout",
            "view",
            "add",
            "edit",
            "delete",
            "review",
            "list",
            "search",
        };
        private readonly string[] types = { "film", "series", "actor" };
        private readonly string[] statuses = { "completed", "watching", "planning" };
        private readonly string[] subcommands = { "add", "delete" };

        private string typesString => string.Join(", ", types);
        private string statusesString => string.Join(", ", statuses);
        private string subcommandsString => string.Join(", ", subcommands);

        public void ExecuteCommand(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new InvalidCommandException($"Type '{appName} help' to see available commands.");
                }

                // Declaring variable to prevent scope issues in switch statements
                string command = args[0].ToLower();
                string type;
                string subcommand;
                int id;
                string username;
                List<Film> films;
                List<Series> seriesList;
                List<Actor> actors;

                string[] reviewTypes = { "film", "series" };

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
                                case "stats":
                                    Console.WriteLine($"{appName} {cmd} - Display statistics about the catalogue.");
                                    break;
                                case "login":
                                    Console.WriteLine($"{appName} {cmd} - Log in to the application.");
                                    break;
                                case "logout":
                                    Console.WriteLine($"{appName} {cmd} - Log out from the application.");
                                    break;
                                case "view":
                                    Console.WriteLine($"{appName} {cmd} <type> <id> - View details of a record ({typesString}) in the catalogue.");
                                    break;
                                case "add":
                                    Console.WriteLine($"{appName} {cmd} <type> - Add a new record ({typesString}) to the catalogue.");
                                    break;
                                case "edit":
                                    Console.WriteLine($"{appName} {cmd} <type> <id> - Edit details of a record ({typesString}) in the catalogue.");
                                    break;
                                case "delete":
                                    Console.WriteLine($"{appName} {cmd} <type> <id> - Delete a record ({typesString}) from the catalogue.");
                                    break;
                                case "review":
                                    Console.WriteLine($"{appName} {cmd} - Manage reviews for films and series.");
                                    Console.WriteLine($"  {appName} {cmd} add <type> <id> - Add a review for a show (film or series) with the specified ID.");
                                    Console.WriteLine($"  {appName} {cmd} delete <type> <id> - Delete a review for a show (film or series) with the specified ID.");
                                    break;
                                case "list":
                                    Console.WriteLine($"{appName} {cmd} - Manage your personal list of shows.");
                                    Console.WriteLine($"  {appName} {cmd} add <type> <id> <status> - Add a show to your list with the specified status ({statusesString}).");
                                    Console.WriteLine($"  {appName} {cmd} delete <type> <id> - Delete a show with the specified ID from your list.");
                                    break;
                                case "search":
                                    Console.WriteLine($"{appName} {cmd} <type> <type:value> - Search and display shows or actors.");
                                    Console.WriteLine(" Film search types:");
                                    Console.WriteLine(" - sort (rating, release, length)");
                                    Console.WriteLine(" - title");
                                    Console.WriteLine(" - genres (comma-separated in quotes)");
                                    Console.WriteLine(" - rating (min-max)");
                                    Console.WriteLine(" - studio");
                                    Console.WriteLine(" - director");
                                    Console.WriteLine(" - actors (comma-separated in quotes)");
                                    Console.WriteLine(" - length (min-max in minutes)");
                                    Console.WriteLine(" - release (min-max in YYYY format)");

                                    Console.WriteLine(" Series search types:");
                                    Console.WriteLine(" - sort (rating, startdate, seasons, length)");
                                    Console.WriteLine(" - title");
                                    Console.WriteLine(" - genres (comma-separated in quotes)");
                                    Console.WriteLine(" - rating (min-max)");
                                    Console.WriteLine(" - studio");
                                    Console.WriteLine(" - director");
                                    Console.WriteLine(" - actors (comma-separated in quotes)");
                                    Console.WriteLine(" - length (min-max in minutes)");
                                    Console.WriteLine(" - seasons (min-max)");
                                    Console.WriteLine(" - date (min-max in YYYY format)");

                                    Console.WriteLine(" Actor search types:");
                                    Console.WriteLine(" - sort (dob)");
                                    Console.WriteLine(" - name");
                                    Console.WriteLine(" - nationality");
                                    Console.WriteLine(" - show");
                                    Console.WriteLine(" - dob (min-max in YYYY format)");
                                    break;
                                default:
                                    Console.WriteLine($"Invalid command: {cmd}");
                                    break;
                            }
                        }
                        break;
                    case "stats":
                        films = DataStorage.LoadFilms();
                        seriesList = DataStorage.LoadSeries();
                        actors = DataStorage.LoadActors();
                        //var listItems = DataStorage.LoadListItems();

                        Console.WriteLine("Statistics: ");
                        Console.WriteLine($"  Films: {films.Count}");
                        Console.WriteLine($"  Series: {seriesList.Count}");
                        Console.WriteLine($"  Actors: {actors.Count}");
                        //Console.WriteLine($"  List items: {listItems.Count}");

                        var reviews = films
                            .SelectMany(f => f.Reviews)
                            .Concat(seriesList.SelectMany(s => s.Reviews));
                        Console.WriteLine($"  Reviews: {reviews.Count()}");
                        Console.WriteLine($"  Average rating: {reviews.Average(r => r.Rating)}");

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
                    case "view":
                        // Check if type and ID are specified
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
                        }

                        type = args[1].ToLower();

                        // Check if type is valid
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        id = int.Parse(args[2]);

                        if (type == "film")
                        {
                            var film = DataStorage.LoadFilm(id);
                            film.Output();
                        }
                        else if (type == "series")
                        {
                            var series = DataStorage.LoadSeries(id);
                            series.Output();
                        }
                        else if (type == "actor")
                        {
                            var actor = DataStorage.LoadActor(id);
                            actor.Output();
                        }

                        break;
                    case "add":
                        // Check if type is specified
                        if (args.Length < 2)
                        {
                            throw new MediaTypeException(types);
                        }

                        type = args[1].ToLower();

                        // Check if type is valid
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                            var show = InputFilm();
                            DataStorage.SaveFilm(show);
                            Console.WriteLine("Film added.");
                        }
                        else if (type == "series")
                        {
                            var show = InputSeries();
                            DataStorage.SaveSeries(show);
                            Console.WriteLine("Series added.");
                        }
                        else if (type == "actor")
                        {
                            var person = InputActor();
                            DataStorage.SaveActor(person);
                            Console.WriteLine("Actor added.");
                        }
                        break;
                    case "edit":
                        // Check if type and ID are specified
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
                        }

                        type = args[1].ToLower();
                        id = int.Parse(args[2]);

                        // Check if type is valid
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
                        // Check if type and ID are specified
                        if (args.Length < 3)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
                        }

                        type = args[1].ToLower();
                        id = int.Parse(args[2]);

                        // Check if type is valid
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        if (type == "film")
                        {
                            DataStorage.DeleteFilm(id);
                            Console.WriteLine("Film deleted.");
                        }
                        else if (type == "series")
                        {
                            DataStorage.DeleteSeries(id);
                            Console.WriteLine("Series deleted.");
                        }
                        else if (type == "actor")
                        {
                            DataStorage.DeleteActor(id);
                            Console.WriteLine("Actor deleted.");
                        }
                        break;
                    case "review":
                        // Check if subcommand, type and ID are specified
                        if (args.Length < 4)
                        {
                            throw new InvalidCommandException($"Please specify a subcommand (add, delete), type ({typesString}), and ID.");
                        }

                        subcommand = args[1].ToLower();
                        type = args[2].ToLower();
                        id = int.Parse(args[3]);

                        // Check if subcommand is valid
                        if (!subcommands.Contains(subcommand))
                        {
                            throw new InvalidCommandException($"Invalid subcommand. Available subcommands: {subcommandsString}");
                        }
                        // Check if type is valid
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(reviewTypes);
                        }

                        // Check if user is logged in
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
                                show.DeleteReview(username); // Delete review if it already exists
                                show.AddReview(review);
                                DataStorage.UpdateFilm(show);
                            }
                            else if (type == "series")
                            {
                                var show = DataStorage.LoadSeries(id);
                                var review = InputReview(username);
                                show.DeleteReview(username); // Delete review if it already exists
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
                    case "list":
                        // Use extended subcommands to allow for "list view" command
                        string[] extendedSubcommands = { "add", "delete", "view" };
                        string extSubString = string.Join(", ", extendedSubcommands);

                        // Check if user is logged in
                        username = DataStorage.LoadUsername();
                        if (string.IsNullOrWhiteSpace(username))
                        {
                            throw new UnauthorizedAccessException("Please log in to use this command.");
                        }

                        // Check if subcommand is specified
                        if (args.Length == 1)
                        {
                            throw new InvalidCommandException($"Please specify a subcommand ({extSubString}).");
                        }
                        subcommand = args[1].ToLower();

                        // Check if subcommand is valid
                        if (!extendedSubcommands.Contains(subcommand))
                        {
                            throw new InvalidCommandException($"Invalid subcommand. Available subcommands: {extSubString}");
                        }

                        // If subcommand is "view", display the user's list
                        if (subcommand == "view")
                        {
                            var userList = DataStorage.LoadUserList(username);
                            if (userList.Count == 0)
                            {
                                Console.WriteLine("Your list is empty.");
                            }

                            userList
                                .OrderBy(item => item.Status == "completed")
                                .ToList()
                                .ForEach(item =>
                            {
                                ConsoleColor statusColor;

                                switch (item.Status)
                                {
                                    case "completed":
                                        statusColor = ConsoleColor.Cyan;
                                        break;
                                    case "watching":
                                        statusColor = ConsoleColor.Blue;
                                        break;
                                    case "planning":
                                        statusColor = ConsoleColor.Yellow;
                                        break;
                                    default:
                                        statusColor = ConsoleColor.White;
                                        break;
                                }

                                Console.ForegroundColor = statusColor;
                                Console.WriteLine($"  Type: {item.MediaType}, ID: {item.MediaId}, Title: \"{item.MediaTitle}\", Status: {item.Status}");
                                Console.ResetColor();
                            });

                            return;
                        }

                        // Check if type and ID are specified
                        if (args.Length < 4)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}) and ID.");
                        }

                        type = args[2].ToLower();
                        id = int.Parse(args[3]);

                        // Check if type is valid
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(reviewTypes);
                        }

                        if (subcommand == "add")
                        {
                            // Check if status is specified
                            if (args.Length < 5)
                            {
                                throw new InvalidCommandException($"Please specify a status ({statusesString}).");
                            }

                            string status = args[4].ToLower();

                            // Check if status is valid
                            if (!statuses.Contains(status))
                            {
                                throw new InvalidCommandException($"Invalid status. Available statuses: ({statusesString})");
                            }

                            // If the list item already exists, update the status
                            var existingItem = DataStorage.LoadListItem(username, type, id);
                            if (existingItem != null)
                            {
                                existingItem.Status = status;
                                Console.WriteLine("List item updated.");
                                return;
                            }

                            if (type == "film")
                            {
                                var show = DataStorage.LoadFilm(id);
                                var listItem = new ListItem(username, "film", show.Id, show.Title, status);
                                DataStorage.SaveListItem(listItem);
                            }
                            else if (type == "series")
                            {
                                var show = DataStorage.LoadSeries(id);
                                var listItem = new ListItem(username, "series", show.Id, show.Title, status);
                                DataStorage.SaveListItem(listItem);
                            }
                            Console.WriteLine("List item added.");
                        }
                        else if (subcommand == "delete")
                        {
                            DataStorage.DeleteListItem(username, type, id);
                            Console.WriteLine("List item deleted.");
                        }
                        break;
                    case "search":
                        // Check if type is specified
                        if (args.Length < 2)
                        {
                            throw new InvalidCommandException($"Please specify a type ({typesString}).");
                        }

                        // Check if type is valid
                        type = args[1].ToLower();
                        if (!types.Contains(type))
                        {
                            throw new MediaTypeException(types);
                        }

                        Dictionary<string, string> filters = ParseSearchFilters(args.Skip(2));

                        switch (type)
                        {
                            case "film":
                                films = DataStorage.LoadFilms();

                                foreach (var filter in filters)
                                {
                                    switch (filter.Key.ToLower())
                                    {
                                        case "sort":
                                            switch (filter.Value.ToLower())
                                            {
                                                case "rating":
                                                    films = films.OrderByDescending(f => f.AvgRating).ToList();
                                                    break;
                                                case "release":
                                                    films = films.OrderBy(f => f.ReleaseDate).ToList();
                                                    break;
                                                case "length":
                                                    films = films.OrderByDescending(f => f.EpisodeLength).ToList();
                                                    break;
                                                default:
                                                    Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                                    break;
                                            }
                                            break;
                                        case "title":
                                            films = films.Where(f => f.Title.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "genres":
                                            var genres = ParseCommaSeparated(filter.Value);
                                            films = films.Where(f => genres.All(genre => f.Genres.Contains(genre))).ToList();
                                            break;
                                        case "rating":
                                            var (minRating, maxRating) = ParseRange(filter.Value);
                                            films = films.Where(f => f.AvgRating >= minRating && f.AvgRating <= maxRating).ToList();
                                            break;
                                        case "studio":
                                            films = films.Where(f => f.Studio.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "director":
                                            films = films.Where(f => f.Director.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "actors":
                                            var parsedActors = ParseCommaSeparated(filter.Value);
                                            films = films.Where(f => parsedActors.All(actor => f.Actors.Contains(actor))).ToList();
                                            break;
                                        case "length":
                                            var (minLength, maxLength) = ParseRange(filter.Value);
                                            films = films.Where(f => f.EpisodeLength != null && f.EpisodeLength >= minLength && f.EpisodeLength <= maxLength).ToList();
                                            break;
                                        case "release":
                                            var (minReleaseDate, maxReleaseDate) = ParseDateRange(filter.Value);
                                            films = films
                                                .Where(f => f.ReleaseDate.HasValue && IsDateInRange(f.ReleaseDate.Value, minReleaseDate, maxReleaseDate))
                                                .ToList();
                                            break;
                                        default:
                                            Console.WriteLine($"Unknown filter: {filter.Key}");
                                            break;
                                    }
                                }

                                DisplayFilmResults(films);
                                break;
                            case "series":
                                seriesList = DataStorage.LoadSeries();

                                foreach (var filter in filters)
                                {
                                    switch (filter.Key.ToLower())
                                    {
                                        case "sort":
                                            switch (filter.Value.ToLower())
                                            {
                                                case "rating":
                                                    seriesList = seriesList.OrderByDescending(s => s.AvgRating).ToList();
                                                    break;
                                                case "startdate":
                                                    seriesList = seriesList.OrderBy(s => s.StartDate).ToList();
                                                    break;
                                                case "seasons":
                                                    seriesList = seriesList.OrderByDescending(s => s.Seasons).ToList();
                                                    break;
                                                case "length":
                                                    seriesList = seriesList.OrderByDescending(s => s.EpisodeLength).ToList();
                                                    break;
                                                default:
                                                    Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                                    break;
                                            }
                                            break;
                                        case "title":
                                            seriesList = seriesList.Where(s => s.Title.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "genres":
                                            var genres = ParseCommaSeparated(filter.Value);
                                            seriesList = seriesList.Where(s => genres.All(genre => s.Genres.Contains(genre))).ToList();
                                            break;
                                        case "rating":
                                            var (minRating, maxRating) = ParseRange(filter.Value);
                                            seriesList = seriesList.Where(s => s.AvgRating >= minRating && s.AvgRating <= maxRating).ToList();
                                            break;
                                        case "studio":
                                            seriesList = seriesList.Where(s => s.Studio.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "director":
                                            seriesList = seriesList.Where(s => s.Director.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "actors":
                                            var parsedActors = ParseCommaSeparated(filter.Value);
                                            seriesList = seriesList.Where(s => parsedActors.All(actor => s.Actors.Contains(actor))).ToList();
                                            break;
                                        case "length":
                                            var (minLength, maxLength) = ParseRange(filter.Value);
                                            seriesList = seriesList.Where(s => s.EpisodeLength != null && s.EpisodeLength >= minLength && s.EpisodeLength <= maxLength).ToList();
                                            break;
                                        case "seasons":
                                            var (minSeasons, maxSeasons) = ParseRange(filter.Value);
                                            seriesList = seriesList
                                                .Where(s => s.Seasons != null && s.Seasons >= minSeasons && s.Seasons <= maxSeasons).ToList();
                                            break;
                                        case "date":
                                            var (minStartDate, maxStartDate) = ParseDateRange(filter.Value);
                                            seriesList = seriesList
                                                .Where(s => s.StartDate.HasValue && IsDateInRange(s.StartDate.Value, minStartDate, maxStartDate))
                                                .ToList();
                                            break;
                                        default:
                                            Console.WriteLine($"Unknown filter: {filter.Key}");
                                            break;
                                    }
                                }

                                DisplaySeriesResults(seriesList);
                                break;
                            case "actor":
                                actors = DataStorage.LoadActors();

                                foreach (var filter in filters)
                                {
                                    switch (filter.Key.ToLower())
                                    {
                                        case "sort":
                                            switch (filter.Value.ToLower())
                                            {
                                                case "dob":
                                                    actors = actors.OrderBy(a => a.DateOfBirth).ToList();
                                                    break;
                                                default:
                                                    Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                                    break;
                                            }
                                            break;
                                        case "name":
                                            actors = actors.Where(a => a.FullName.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "nationality":
                                            actors = actors.Where(a => a.Nationality.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                                            break;
                                        case "show":
                                            actors = actors.Where(a => a.Filmography.Any(show => show.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))).ToList();
                                            break;
                                        case "dob":
                                            var (minDob, maxDob) = ParseDateRange(filter.Value);
                                            actors = actors
                                                .Where(a => IsDateInRange(a.DateOfBirth, minDob, maxDob))
                                                .ToList();
                                            break;
                                        default:
                                            Console.WriteLine($"Unknown filter: {filter.Key}");
                                            break;
                                    }
                                }

                                DisplayActorResults(actors);
                                break;
                        }
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
        private Dictionary<string, string> ParseSearchFilters(IEnumerable<string> filterArgs)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();

            foreach (var arg in filterArgs)
            {
                var parts = arg.Split(':');
                if (parts.Length == 2)
                {
                    filters[parts[0].ToLower()] = parts[1].Trim();
                }
            }

            return filters;
        }
        private void DisplayFilmResults(IEnumerable<Film> results)
        {
            DisplaySearchResults("Film", results, f => $"{f.Id}: {f.Title}");
        }
        private void DisplaySeriesResults(IEnumerable<Series> results)
        {
            DisplaySearchResults("Series", results, s => $"{s.Id}: {s.Title}");
        }
        private void DisplayActorResults(IEnumerable<Actor> results)
        {
            DisplaySearchResults("Actor", results, a => $"{a.Id}: {a.FullName}");
        }
        private void DisplaySearchResults<T>(string type, IEnumerable<T> results, Func<T, string> formatFunc)
        {
            if (results.Any())
            {
                Console.WriteLine($"Search Results ({type}):");
                foreach (var result in results)
                {
                    Console.WriteLine(formatFunc(result));
                }
            }
            else
            {
                Console.WriteLine($"No {type.ToLower()} found.");
            }
        }
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
        private static (int, int) ParseRange(string range)
        {
            var parts = range.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[0], out int min) && int.TryParse(parts[1], out int max))
            {
                return (min, max);
            }
            return (1, 10);
        }
        private static List<string> ParseCommaSeparated(string commaSeparated)
        {
            return commaSeparated.Split(',').Select(a => a.Trim()).ToList();
        }
        private static bool IsDateInRange(DateOnly date, DateOnly minDate, DateOnly maxDate)
        {
            return date.CompareTo(minDate) >= 0 && date.CompareTo(maxDate) <= 0;
        }
        private static (DateOnly, DateOnly) ParseDateRange(string dateRange)
        {
            var parts = dateRange.Split('-');
    
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int startYear) &&
                int.TryParse(parts[1], out int endYear) &&
                startYear <= endYear)
            {
                DateOnly minDate = new DateOnly(startYear, 1, 1);
                DateOnly maxDate = new DateOnly(endYear, 12, 31);

                return (minDate, maxDate);
            }

            return (default, default);
        }
    }
}

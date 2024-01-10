using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Search
    {
        string type;
        Dictionary<string, string> filters;

        public Search(string[] args, string[] types)
        {
            string typesString = string.Join(", ", types);

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

            filters = ParseSearchFilters(args.Skip(2));

            switch (type)
            {
                case "film":
                    SearchFilms();
                    break;
                case "series":
                    SearchSeries();
                    break;
                case "actor":
                    SearchActors();
                    break;
            }
        }
        private void SearchFilms()
        {
            var films = DataStorage.LoadFilms();

            foreach (var filter in filters)
            {
                switch (filter.Key.ToLower())
                {
                    case "sort":
                        switch (filter.Value.ToLower())
                        {
                            case "rating":
                                films = films
                                    .AsParallel()
                                    .OrderByDescending(f => f.AvgRating)
                                    .ToList();
                                break;
                            case "release":
                                films = films
                                    .AsParallel()
                                    .OrderBy(f => f.ReleaseDate)
                                    .ToList();
                                break;
                            case "length":
                                films = films
                                    .AsParallel()
                                    .OrderByDescending(f => f.EpisodeLength)
                                    .ToList();
                                break;
                            default:
                                Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                break;
                        }
                        break;
                    case "title":
                        films = films
                            .AsParallel()
                            .Where(f => f.Title.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "genres":
                        var genres = ParseCommaSeparated(filter.Value);
                        films = films
                            .AsParallel()
                            .Where(f => genres.All(genre => f.Genres.Contains(genre)))
                            .ToList();
                        break;
                    case "rating":
                        var (minRating, maxRating) = ParseRange(filter.Value);
                        films = films
                            .AsParallel()
                            .Where(f => f.AvgRating >= minRating && f.AvgRating <= maxRating)
                            .ToList();
                        break;
                    case "studio":
                        films = films
                            .AsParallel()
                            .Where(f => f.Studio.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "director":
                        films = films
                            .AsParallel()
                            .Where(f => f.Director.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "actors":
                        var parsedActors = ParseCommaSeparated(filter.Value);
                        films = films
                            .AsParallel()
                            .Where(f => parsedActors.All(actor => f.Actors.Contains(actor)))
                            .ToList();
                        break;
                    case "length":
                        var (minLength, maxLength) = ParseRange(filter.Value);
                        films = films
                            .AsParallel()
                            .Where(f => f.EpisodeLength != null && f.EpisodeLength >= minLength && f.EpisodeLength <= maxLength)
                            .ToList();
                        break;
                    case "release":
                        var (minReleaseDate, maxReleaseDate) = ParseDateRange(filter.Value);
                        films = films
                            .AsParallel()
                            .Where(f => f.ReleaseDate.HasValue && IsDateInRange(f.ReleaseDate.Value, minReleaseDate, maxReleaseDate))
                            .ToList();
                        break;
                    default:
                        Console.WriteLine($"Unknown filter: {filter.Key}");
                        break;
                }
            }

            DisplayFilmResults(films);
        }
        private void SearchSeries()
        {
            var seriesList = DataStorage.LoadSeries();

            foreach (var filter in filters)
            {
                switch (filter.Key.ToLower())
                {
                    case "sort":
                        switch (filter.Value.ToLower())
                        {
                            case "rating":
                                seriesList = seriesList
                                    .AsParallel()
                                    .OrderByDescending(s => s.AvgRating)
                                    .ToList();
                                break;
                            case "startdate":
                                seriesList = seriesList
                                    .AsParallel()
                                    .OrderBy(s => s.StartDate)
                                    .ToList();
                                break;
                            case "seasons":
                                seriesList = seriesList
                                    .AsParallel()
                                    .OrderByDescending(s => s.Seasons)
                                    .ToList();
                                break;
                            case "length":
                                seriesList = seriesList
                                    .AsParallel()
                                    .OrderByDescending(s => s.EpisodeLength)
                                    .ToList();
                                break;
                            default:
                                Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                break;
                        }
                        break;
                    case "title":
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.Title.Contains(filter.Value,
                            StringComparison.OrdinalIgnoreCase)) // Ignore case Ref:https://stackoverflow.com/a/444818
                            .ToList();
                        break;
                    case "genres":
                        var genres = ParseCommaSeparated(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => genres.All(genre => s.Genres.Contains(genre)))
                            .ToList();
                        break;
                    case "rating":
                        var (minRating, maxRating) = ParseRange(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.AvgRating >= minRating && s.AvgRating <= maxRating)
                            .ToList();
                        break;
                    case "studio":
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.Studio.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "director":
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.Director.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "actors":
                        var parsedActors = ParseCommaSeparated(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => parsedActors.All(actor => s.Actors.Contains(actor)))
                            .ToList();
                        break;
                    case "length":
                        var (minLength, maxLength) = ParseRange(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.EpisodeLength != null && s.EpisodeLength >= minLength && s.EpisodeLength <= maxLength)
                            .ToList();
                        break;
                    case "seasons":
                        var (minSeasons, maxSeasons) = ParseRange(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.Seasons != null && s.Seasons >= minSeasons && s.Seasons <= maxSeasons)
                            .ToList();
                        break;
                    case "date":
                        var (minStartDate, maxStartDate) = ParseDateRange(filter.Value);
                        seriesList = seriesList
                            .AsParallel()
                            .Where(s => s.StartDate.HasValue && IsDateInRange(s.StartDate.Value, minStartDate, maxStartDate))
                            .ToList();
                        break;
                    default:
                        Console.WriteLine($"Unknown filter: {filter.Key}");
                        break;
                }
            }

            DisplaySeriesResults(seriesList);
        }
        private void SearchActors()
        {
            var actors = DataStorage.LoadActors();

            foreach (var filter in filters)
            {
                switch (filter.Key.ToLower())
                {
                    case "sort":
                        switch (filter.Value.ToLower())
                        {
                            case "dob":
                                actors = actors
                                    .AsParallel()
                                    .OrderBy(a => a.DateOfBirth)
                                    .ToList();
                                break;
                            default:
                                Console.WriteLine($"Unknown sort criteria: {filter.Value}");
                                break;
                        }
                        break;
                    case "name":
                        actors = actors
                            .AsParallel()
                            .Where(a => a.FullName.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "nationality":
                        actors = actors
                            .AsParallel()
                            .Where(a => a.Nationality.Contains(filter.Value, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case "show":
                        actors = actors
                            .AsParallel()
                            .Where(a => a.Filmography.Any(show => show.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                        break;
                    case "dob":
                        var (minDob, maxDob) = ParseDateRange(filter.Value);
                        actors = actors
                            .AsParallel()
                            .Where(a => IsDateInRange(a.DateOfBirth, minDob, maxDob))
                            .ToList();
                        break;
                    default:
                        Console.WriteLine($"Unknown filter: {filter.Key}");
                        break;
                }
            }

            DisplayActorResults(actors);
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
    }
}

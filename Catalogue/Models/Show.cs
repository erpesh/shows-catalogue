using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    abstract class Show
    {
        private readonly string showsFolder = "shows";
        private string FilePath => Path.Combine(showsFolder, $"{title}.json");

        protected string title;
        protected string? description;
        protected List<string> genres;
        protected List<string> studios;
        protected Person director;
        protected List<ShowRating> ratings = new List<ShowRating> { };
        protected double? avgRating;
        protected List<Actor> actors;
        protected int? episodeLength;

        public string Title
        {
            get => title;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value.Length > 1000)
                    throw new ArgumentException("The title is too long.");
                title = value;
            }
        }
        public string? Description
        {
            get => description;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value.Length > 1000)
                    throw new ArgumentException("The title is too long.");
                description = value;
            }
        }
        public List<string> Genres
        {
            get => genres;
            set => genres = value;
        }
        public List<string> Studios
        {
            get => studios;
            set => studios = value;
        }
        public Person Director
        {
            get => director;
            set => director = value;
        }
        public List<ShowRating> Ratings
        {
            get => ratings;
            set => ratings = value;
        }
        public double? AvgRating
        {
            get => avgRating;
            set => avgRating = value;
        }
        public List<Actor> Actors
        {
            get => actors;
            set => actors = value;
        }
        public int? EpisodeLength
        {
            get => episodeLength;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value <= 0)
                    throw new ArgumentException("Episode length can be only a positive number in minutes.");
                episodeLength = value;
            }
        }

        public Show(string title)
        {
            Title = title;
            LoadFromFile();
        }

        public Show(
            string title, 
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            List<Actor> actors,
            int episodeLength
        )
        {
            Title = title;
            Description = description;
            Genres = genres;
            Studios = studios;
            Director = director;
            Actors = actors;
            EpisodeLength = episodeLength;

            UpdateFile();
        }

        protected void UpdateFile()
        {
            SaveToFile();
        }

        public void AddRating(User user, int rating)
        {
            var showRating = new ShowRating(user.Username, rating);
            ratings.Add(showRating);
            UpdateFile(); // Save changes to file after adding new ratings
        }

        private void SaveToFile()
        {
            string directoryPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        protected void LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var loadedShow = JsonConvert.DeserializeObject<Show>(json);

                // Copy properties from loaded show to the current instance
                CopyProperties(loadedShow);
            }
            else
            {
                // Handle the case when the file does not exist
            }
        }

        protected virtual void CopyProperties(Show source)
        {
            Title = source.Title;
            Description = source.Description;
            Genres = source.Genres;
            Studios = source.Studios;
            Director = source.Director;
            Ratings = source.Ratings;
            AvgRating = source.AvgRating;
            Actors = source.Actors;
            EpisodeLength = source.EpisodeLength;
        }
    }
}

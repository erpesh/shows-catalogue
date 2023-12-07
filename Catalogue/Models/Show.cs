using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public abstract class Show
    {
        protected int id;
        protected string title;
        protected string? description;
        protected List<string> genres;
        protected string studio;
        protected Person director;
        protected List<ShowRating> ratings = new List<ShowRating> { };
        protected double? avgRating;
        protected List<Actor> actors;
        protected int? episodeLength;

        public int Id
        {
            get => id;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value < 1)
                    throw new ArgumentException("ID can't be less than 1");
                id = value;
            }
        }
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
        public string Studio
        {
            get => studio;
            set => studio = value;
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

        public Show(int _id)
        {
            LoadFromFile();
        }

        public Show(
            string _title, 
            string _description,
            List<string> _genres,
            string _studio,
            Person _director,
            List<Actor> _actors,
            int _episodeLength
        )
        {
            Title = _title;
            Description = _description;
            Genres = _genres;
            Studio = _studio;
            Director = _director;
            Actors = _actors;
            EpisodeLength = _episodeLength;
        }

        protected void UpdateFile()
        {
            SaveToFile();
        }

        public void AddRating(User user, int rating)
        {
            var showRating = new ShowRating(user.Username, rating);
            ratings.Add(showRating);

            AvgRating = CalculateAvgRating();

            UpdateFile();
        }

        protected double CalculateAvgRating()
        {
            if (Ratings.Count == 0) return 0;
            
            double totalRating = Ratings.Sum(r => r.Rating);
            double avgRating = totalRating / Ratings.Count;

            return avgRating;
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
            Id = source.Id;
            Title = source.Title;
            Description = source.Description;
            Genres = source.Genres;
            Studio = source.Studio;
            Director = source.Director;
            Ratings = source.Ratings;
            AvgRating = source.AvgRating;
            Actors = source.Actors;
            EpisodeLength = source.EpisodeLength;
        }
    }
}

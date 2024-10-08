﻿using System;
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
        protected string director;
        protected List<Review> reviews = new List<Review> { };
        protected double? avgRating;
        protected List<string> actors;
        protected int? episodeLength;

        public int Id
        {
            get => id;
            set
            {
                if (value < 1)
                    throw new ArgumentException("ID can't be less than 1");
                id = value;
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                    throw new ArgumentException("The title is too short.");
                if (value.Length > 100)
                    throw new ArgumentException("The title is too long.");
                title = value;
            }
        }
        public string? Description
        {
            get => description;
            set
            {
                if (value != null && value.Length < 3)
                    throw new ArgumentException("The description is too short.");
                if (value != null && value.Length > 1000)
                    throw new ArgumentException("The description is too long.");
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
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                    throw new ArgumentException("The studio name is too short.");
                if (value.Length > 100)
                    throw new ArgumentException("The studio name is too long.");
                studio = value;
            }
        }
        public string Director
        {
            get => director;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                    throw new ArgumentException("The studio name is too short.");
                if (value.Length > 100)
                    throw new ArgumentException("The studio name is too long.");
                director = value;
            }
        }
        public List<Review> Reviews
        {
            get => reviews;
            set => reviews = value;
        }
        public double? AvgRating
        {
            get => avgRating;
            set => avgRating = value;
        }
        public List<string> Actors
        {
            get => actors;
            set => actors = value;
        }
        public int? EpisodeLength
        {
            get => episodeLength;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Episode length can be only a positive number in minutes.");
                if (value > 1000)
                    throw new ArgumentException("Episode length can't be longer than 1000 minutes.");
                episodeLength = value;
            }
        }

        public Show(
            string _title, 
            string? _description,
            List<string> _genres,
            string _studio,
            string _director,
            List<string> _actors,
            int? _episodeLength
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

        public void AddReview(Review review)
        {
            Reviews.Add(review);
            AvgRating = CalculateAvgRating();
        }
        public void DeleteReview(string username)
        {
            Review review = Reviews.Find(r => r.Username == username);
            if (review != null)
            {
                Reviews.Remove(review);
                AvgRating = CalculateAvgRating();
            }
        }
        protected double? CalculateAvgRating()
        {
            if (Reviews.Count == 0) return null;
            
            double totalRating = Reviews.AsParallel().Sum(r => r.Rating);
            double avgRating = totalRating / Reviews.Count;

            return avgRating;
        }
        protected void OutputReviews()
        {
            if (Reviews.Count == 0)
            {
                Console.WriteLine("No reviews yet.");
                return;
            }

            Console.WriteLine("Reviews:");
            foreach (Review review in Reviews)
            {
                review.Output();
            }
        }
        public virtual void Output()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Genres: {string.Join(", ", Genres)}");
            Console.WriteLine($"Studio: {Studio}");
            Console.WriteLine($"Director: {Director}");
            Console.WriteLine($"Actors: {string.Join(", ", Actors)}");
            Console.WriteLine($"Episode length: {EpisodeLength} minutes");
            Console.WriteLine($"Average rating: {AvgRating}");
        }
    }
}

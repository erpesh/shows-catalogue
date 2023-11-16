using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Series : Show
    {
        private int? seasons;
        private int? episodes;
        private DateOnly? startDate;
        private DateOnly? endDate;

        public int? Seasons
        {
            get => seasons;
            set => seasons = value;
        }
        public int? Episodes
        {
            get => episodes;
            set => episodes = value;
        }
        public DateOnly? StartDate
        {
            get => startDate;
            set => startDate = value;
        }
        public DateOnly? EndDate
        {
            get => endDate;
            set => endDate = value;
        }

        public Series(
            string title,
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            List<Actor> actors,
            int episodeLength,
            int seasons, 
            int episodes, 
            DateOnly startDate, 
            DateOnly endDate
        )
            : base(title, description, genres, studios, director, actors, episodeLength)
        {
            Seasons = seasons;
            Episodes = episodes;
            StartDate = startDate;
            EndDate = endDate;
        }

        protected override void CopyProperties(Show source)
        {
            base.CopyProperties(source);
            if (source is Series seriesSource)
            {
                Seasons = seriesSource.Seasons;
                Episodes = seriesSource.Episodes;
                StartDate = seriesSource.StartDate;
                EndDate = seriesSource.EndDate;
            }
        }
    }
}

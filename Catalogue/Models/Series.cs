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
            string _title,
            string _description,
            List<string> _genres,
            string _studio,
            Person _director,
            List<Actor> _actors,
            int _episodeLength,
            int _seasons, 
            int _episodes, 
            DateOnly _startDate, 
            DateOnly _endDate
        )
            : base(_title, _description, _genres, _studio, _director, _actors, _episodeLength)
        {
            Seasons = _seasons;
            Episodes = _episodes;
            StartDate = _startDate;
            EndDate = _endDate;
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

            //UpdateFile();
        }
    }
}

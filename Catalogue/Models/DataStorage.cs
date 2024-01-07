using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public static class DataStorage
    {
        private const string FilmsFilePath = "films.json";
        private const string SeriesFilePath = "series.json";
        private const string StaffMembersFilePath = "staffMembers.json";
        private const string ActorsFilePath = "actors.json";

        public static List<Film> LoadFilms()
        {
            return LoadEntities<Film>(FilmsFilePath);
        }
        public static List<Series> LoadSeries()
        {
            return LoadEntities<Series>(SeriesFilePath);
        }
        public static List<Person> LoadStaff()
        {
            return LoadEntities<Person>(StaffMembersFilePath);
        }
        public static void SaveFilm(Film film)
        {
            List<Film> films = LoadFilms();
            film.Id = GetNextId(films);
            films.Add(film);
            SaveEntities(films, FilmsFilePath);
        }
        public static void SaveSeries(Series series)
        {
            List<Series> seriesList = LoadSeries();
            series.Id = GetNextId(seriesList);
            seriesList.Add(series);
            SaveEntities(seriesList, SeriesFilePath);
        }

        //public static Show LoadShow(int showId)
        //{
        //    return LoadEntity<Show>(ShowsFilePath, showId);
        //}

        public static void SaveStaffMember(Person person)
        {
            List<Person> staffMembers = LoadEntities<Person>(StaffMembersFilePath);
            person.Id = GetNextId(staffMembers);
            staffMembers.Add(person);
            SaveEntities(staffMembers, StaffMembersFilePath);
        }

        public static Person LoadStaffMember(int personId)
        {
            return LoadEntity<Person>(StaffMembersFilePath, personId);
        }

        public static void SaveActor(Actor actor)
        {
            List<Actor> actors = LoadEntities<Actor>(ActorsFilePath);
            actor.Id = GetNextId(actors);
            actors.Add(actor);
            SaveEntities(actors, ActorsFilePath);
        }

        public static Actor LoadActor(int actorId)
        {
            return LoadEntity<Actor>(ActorsFilePath, actorId);
        }

        private static void SaveEntities<T>(List<T> entities, string filePath)
        {
            string json = JsonConvert.SerializeObject(entities, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private static List<T> LoadEntities<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(json);
            }

            return new List<T>();
        }

        private static T LoadEntity<T>(string filePath, int entityId)
        {
            List<T> entities = LoadEntities<T>(filePath);
            return entities.Find(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
        }

        private static int GetNextId<T>(List<T> entities)
        {
            int nextId = entities.Count > 0 ?
                entities
                .AsParallel()
                .Max(e => (int)e.GetType().GetProperty("Id").GetValue(e)) + 1 : 1;
            return nextId;
        }
    }
}

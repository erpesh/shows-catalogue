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
        private const string ShowsFilePath = "shows.json";
        private const string StaffMembersFilePath = "staffMembers.json";
        private const string ActorsFilePath = "actors.json";

        public static List<Show> LoadShows()
        {
            return LoadEntities<Show>(ShowsFilePath);
        }
        public static void SaveShow(Show show)
        {
            List<Show> shows = LoadShows();
            show.Id = GetNextId(shows);
            shows.Add(show);
            SaveEntities(shows, ShowsFilePath);
        }

        public static Show LoadShow(int showId)
        {
            return LoadEntity<Show>(ShowsFilePath, showId);
        }

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

        private static int GetNextId<T>(List<T> entities)
        {
            int nextId = entities.Count > 0 ? 
                entities
                .AsParallel()
                .Max(e => (int)e.GetType().GetProperty("Id").GetValue(e)) + 1 : 1;
            return nextId;
        }

        private static T LoadEntity<T>(string filePath, int entityId)
        {
            List<T> entities = LoadEntities<T>(filePath);
            return entities.Find(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
        }
    }
}

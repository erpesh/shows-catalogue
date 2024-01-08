using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public static class DataStorage
    {
        private const string UsernameFilePath = "username.dat";
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
        public static void SaveActor(Actor actor)
        {
            List<Actor> actors = LoadEntities<Actor>(ActorsFilePath);
            actor.Id = GetNextId(actors);
            actors.Add(actor);
            SaveEntities(actors, ActorsFilePath);
        }
        public static Film LoadFilm(int filmId)
        {
            return LoadEntity<Film>(FilmsFilePath, filmId, "Film");
        }
        public static Series LoadSeries(int seriesId)
        {
            return LoadEntity<Series>(SeriesFilePath, seriesId, "Series");
        }
        public static Actor LoadActor(int actorId)
        {
            return LoadEntity<Actor>(ActorsFilePath, actorId, "Actor");
        }
        public static void UpdateFilm(Film updatedFilm)
        {
            UpdateEntity(updatedFilm, FilmsFilePath, "Film");
        }

        public static void UpdateSeries(Series updatedSeries)
        {
            UpdateEntity(updatedSeries, SeriesFilePath, "Series");
        }

        public static void UpdateActor(Actor updatedActor)
        {
            UpdateEntity(updatedActor, ActorsFilePath, "Actor");
        }
        public static void DeleteFilm(int filmId)
        {
            DeleteEntity<Film>(filmId, FilmsFilePath, "Film");
        }

        public static void DeleteSeries(int seriesId)
        {
            DeleteEntity<Series>(seriesId, SeriesFilePath, "Series");
        }

        public static void DeleteActor(int actorId)
        {
            DeleteEntity<Actor>(actorId, ActorsFilePath, "Actor");
        }

        private static void SaveEntities<T>(List<T> entities, string filePath)
        {
            string json = JsonSerializer.Serialize(entities);
            File.WriteAllText(filePath, json);
        }

        private static List<T> LoadEntities<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<T>>(json);
            }

            return new List<T>();
        }

        private static T LoadEntity<T>(string filePath, int entityId, string type)
        {
            List<T> entities = LoadEntities<T>(filePath);
            T entity = entities.Find(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
            if (entity == null)
            {
                throw new InvalidOperationException($"{type} with ID {entityId} not found.");
            }
            return entity;
        }
        private static void UpdateEntity<T>(T updatedEntity, string filePath, string type)
        {
            List<T> entities = LoadEntities<T>(filePath);
            int index = entities.FindIndex(e => (int)e.GetType().GetProperty("Id").GetValue(e) == (int)updatedEntity.GetType().GetProperty("Id").GetValue(updatedEntity));

            if (index != -1)
            {
                entities[index] = updatedEntity;
                SaveEntities(entities, filePath);
            }
            else
            {
                throw new InvalidOperationException($"{type} with ID {updatedEntity.GetType().GetProperty("Id").GetValue(updatedEntity)} not found.");
            }
        }
        public static void DeleteEntity<T>(int entityId, string filePath, string type)
        {
            List<T> entities = LoadEntities<T>(filePath);
            int index = entities.FindIndex(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);

            if (index != -1)
            {
                entities.RemoveAt(index);
                SaveEntities(entities, filePath);
            }
            else
            {
                throw new InvalidOperationException($"{type} with ID {entityId} not found.");
            }
        }
        private static int GetNextId<T>(List<T> entities)
        {
            int nextId = entities.Count > 0 ?
                entities
                .AsParallel()
                .Max(e => (int)e.GetType().GetProperty("Id").GetValue(e)) + 1 : 1;
            return nextId;
        }
        public static void SaveUsername(string username)
        {
            string json = JsonSerializer.Serialize(username);
            File.WriteAllText(UsernameFilePath, json);
        }

        public static string LoadUsername()
        {
            if (File.Exists(UsernameFilePath))
            {
                string json = File.ReadAllText(UsernameFilePath);
                return JsonSerializer.Deserialize<string>(json);
            }

            return null;
        }

        public static void RemoveUsername()
        {
            if (File.Exists(UsernameFilePath))
            {
                File.Delete(UsernameFilePath);
            }
        }
    }
}

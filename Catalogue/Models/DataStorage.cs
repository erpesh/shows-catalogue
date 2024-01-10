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
        private const string ActorsFilePath = "actors.json";
        private const string UserListFilePath = "userlist.json";

        public static List<Film> LoadFilms()
        {
            return LoadEntities<Film>(FilmsFilePath);
        }
        public static List<Series> LoadSeries()
        {
            return LoadEntities<Series>(SeriesFilePath);
        }
        public static List<Actor> LoadActors()
        {
            return LoadEntities<Actor>(ActorsFilePath);
        }
        public static List<ListItem> LoadUserList(string username)
        {
            List<ListItem> userItems = LoadEntities<ListItem>(UserListFilePath) ?? new List<ListItem>();
            return userItems.Where(item => item.Username == username).ToList();
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
            List<Actor> actors = LoadActors();
            actor.Id = GetNextId(actors);
            actors.Add(actor);
            SaveEntities(actors, ActorsFilePath);
        }
        public static void SaveListItem(ListItem listItem)
        {
            List<ListItem> userItems = LoadEntities<ListItem>(UserListFilePath) ?? new List<ListItem>();
            userItems.Add(listItem);
            SaveEntities(userItems, UserListFilePath);
        }
        public static Film LoadFilm(int filmId)
        {
            return LoadEntity<Film>(FilmsFilePath, filmId);
        }
        public static Series LoadSeries(int seriesId)
        {
            return LoadEntity<Series>(SeriesFilePath, seriesId);
        }
        public static Actor LoadActor(int actorId)
        {
            return LoadEntity<Actor>(ActorsFilePath, actorId);
        }
        public static ListItem? LoadListItem(string username, string mediaType, int mediaId)
        {
            List<ListItem> userList = LoadUserList(username);
            ListItem listItem = userList.Find(item => item.MediaId == mediaId && item.MediaType == mediaType);
            return listItem;
        }
        public static void UpdateFilm(Film updatedFilm)
        {
            UpdateEntity(updatedFilm, FilmsFilePath);
        }
        public static void UpdateSeries(Series updatedSeries)
        {
            UpdateEntity(updatedSeries, SeriesFilePath);
        }
        public static void UpdateActor(Actor updatedActor)
        {
            UpdateEntity(updatedActor, ActorsFilePath);
        }
        public static void DeleteFilm(int filmId)
        {
            DeleteEntity<Film>(filmId, FilmsFilePath);
        }
        public static void DeleteSeries(int seriesId)
        {
            DeleteEntity<Series>(seriesId, SeriesFilePath);
        }
        public static void DeleteActor(int actorId)
        {
            DeleteEntity<Actor>(actorId, ActorsFilePath);
        }
        public static void DeleteListItem(string username, string mediaType, int mediaId)
        {
            var listItems = LoadUserList(username);
            int index = listItems.FindIndex(item => item.MediaId == mediaId && item.MediaType == mediaType);

            if (index != -1)
            {
                listItems.RemoveAt(index);
                SaveEntities(listItems, UserListFilePath);
            }
            else
            {
                throw new InvalidOperationException($"List item not found.");
            }
        }
        // Generic method to save entities to a file
        private static void SaveEntities<T>(List<T> entities, string filePath)
        {
            string json = JsonSerializer.Serialize(entities);
            File.WriteAllText(filePath, json);
        }
        // Generic method to load entities from a file
        private static List<T> LoadEntities<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<T>>(json);
            }

            return new List<T>();
        }
        // Generic method to load an entity from a file
        private static T LoadEntity<T>(string filePath, int entityId)
        {
            List<T> entities = LoadEntities<T>(filePath);
            T entity = entities.Find(e => (int)e.GetType().GetProperty("Id").GetValue(e) == entityId);
            if (entity == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} with ID {entityId} not found.");
            }
            return entity;
        }
        // Generic method to update an entity in a file
        private static void UpdateEntity<T>(T updatedEntity, string filePath)
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
                throw new InvalidOperationException($"{typeof(T).Name} with ID {updatedEntity.GetType().GetProperty("Id").GetValue(updatedEntity)} not found.");
            }
        }
        // Generic method to delete an entity from a file
        public static void DeleteEntity<T>(int entityId, string filePath)
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
                throw new InvalidOperationException($"{typeof(T).Name} with ID {entityId} not found.");
            }
        }
        // Get the next ID for a new entity
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class ListItem
    {
        private string username;
        private string mediaType;
        private int mediaId;
        private string mediaTitle;
        private string status;
        private DateTime dateAdded;

        public string Username
        {
            get => username;
            set => username = value;
        }
        public string MediaType
        {
            get => mediaType;
            set => mediaType = value;
        }
        public int MediaId
        {
            get => mediaId;
            set => mediaId = value;
        }
        public string MediaTitle
        {
            get => mediaTitle;
            set => mediaTitle = value;
        }
        public string Status
        {
            get => status;
            set => status = value;
        }
        public DateTime DateAdded
        {
            get => dateAdded;
            set => dateAdded = value;
        }
        public ListItem(string _username, string _mediaType, int _mediaId, string _mediaTitle, string _status)
        {
            Username = _username;
            MediaType = _mediaType;
            MediaId = _mediaId;
            MediaTitle = _mediaTitle;
            Status = _status;
            DateAdded = DateTime.Now;
        }

        [JsonConstructor]
        public ListItem(string username, string mediaType, int mediaId, string mediaTitle, string status, DateTime dateAdded)
        {
            Username = username;
            MediaType = mediaType;
            MediaId = mediaId;
            MediaTitle = mediaTitle;
            Status = status;
            DateAdded = dateAdded;
        }
    }
}

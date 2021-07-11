﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Song
{
    public class SongUploadModel
    {
        [JsonIgnore]
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string Genre { get; set; }
        [JsonIgnore]
        public int TimesStreamed { get; set; }
        public string AlbumId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Models.User;

namespace ShantyWebAPI.Models.Search
{
    public class SearchResultModel
    {
        public List<SongGetModel> SongGetModels { get; set; }
        public List<AlbumGetModel> AlbumGetModels { get; set; }
        public List<ArtistGetInfoModel> ArtistGetInfoModels { get; set; }
    }
}

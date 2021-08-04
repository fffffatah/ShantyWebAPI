using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Recommended
{
    public class RecommendedModel
    {
        public List<SongGetModel> SongGetModels { get; set; }
        public List<AlbumGetModel> AlbumGetModels { get; set; }
        public List<ArtistGetInfoModel> ArtistGetInfoModels { get; set; }
    }
}

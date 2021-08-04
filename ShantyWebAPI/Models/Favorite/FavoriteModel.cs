using ShantyWebAPI.Models.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Favorite
{
    public class FavoriteModel
    {
        public List<SongGetModel> SongGetModels { get; set; }
    }
}

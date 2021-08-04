using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShantyWebAPI.Models.Favorite;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Favorite
{
    public class FavoriteDataAccess
    {
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
        }

        public FavoriteModel GetFavorite(string id)
        {
            FavoriteModel favoriteModel = new FavoriteModel();
            favoriteModel.SongGetModels = new List<SongGetModel>();
            var songCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var songResults = songCollection.Find(new BsonDocument()).Limit(50).ToList();
            foreach (BsonDocument result in songResults)
            {
                if (result != null)
                {
                    SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                    favoriteModel.SongGetModels.Add(res);
                }
            }
            return favoriteModel;
        }
    }
}

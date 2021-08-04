using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models.Chart;
using ShantyWebAPI.Models.Recommended;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Models.User;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Recommendation
{
    public class RecommendationDataAccess
    {
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
        }

        public RecommendedModel GetRecommended(string id)
        {
            RecommendedModel recommendedModel = new RecommendedModel();
            recommendedModel.AlbumGetModels = new List<AlbumGetModel>();
            recommendedModel.SongGetModels = new List<SongGetModel>();
            recommendedModel.ArtistGetInfoModels = new List<ArtistGetInfoModel>();
            var songCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var artistCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("artists");
            var albumCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("albums");
            var songResults = songCollection.Find(new BsonDocument()).Limit(15).ToList();
            var albumResults = albumCollection.Find(new BsonDocument()).Limit(15).ToList();
            var artistResults = artistCollection.Find(new BsonDocument()).Limit(15).ToList();
            foreach (BsonDocument result in songResults)
            {
                if (result != null)
                {
                    SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                    recommendedModel.SongGetModels.Add(res);
                }
            }
            foreach (BsonDocument result in albumResults)
            {
                if (result != null)
                {
                    AlbumGetModel res = BsonSerializer.Deserialize<AlbumGetModel>(result);
                    recommendedModel.AlbumGetModels.Add(res);
                }
            }
            foreach (BsonDocument result in artistResults)
            {
                if (result != null)
                {
                    ArtistGetInfoModel res = BsonSerializer.Deserialize<ArtistGetInfoModel>(result);
                    recommendedModel.ArtistGetInfoModels.Add(res);
                }
            }
            return recommendedModel;
        }

        public GlobalTopModel GetGlobalTop(string id)
        {
            GlobalTopModel globalTopModel = new GlobalTopModel();
            globalTopModel.SongGetModels = new List<SongGetModel>();
            var songCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var songResults = songCollection.Find(new BsonDocument()).Limit(50).ToList();
            foreach (BsonDocument result in songResults)
            {
                if (result != null)
                {
                    SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                    globalTopModel.SongGetModels.Add(res);
                }
            }
            return globalTopModel;
        }
    }
}

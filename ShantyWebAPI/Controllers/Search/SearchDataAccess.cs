using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models.Search;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Models.User;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Search
{
    public class SearchDataAccess
    {
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
        }

        public SearchResultModel SearchResult(string query)
        {
            SearchResultModel searchResultModel = new SearchResultModel();
            searchResultModel.AlbumGetModels = new List<AlbumGetModel>();
            searchResultModel.SongGetModels = new List<SongGetModel>();
            searchResultModel.ArtistGetInfoModels = new List<ArtistGetInfoModel>();
            var songCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var artistCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("artists");
            var albumCollection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("albums");
            var songfilter = new BsonDocument { { "SongName", new BsonDocument { { "$regex", query }, { "$options", "i" } } } };
            var songResults = songCollection.Find(songfilter).ToList();
            var albumfilter = new BsonDocument { { "AlbumName", new BsonDocument { { "$regex", query }, { "$options", "i" } } } };
            var albumResults = albumCollection.Find(albumfilter).ToList();
            var artistfilter = new BsonDocument { { "FirstName", new BsonDocument { { "$regex", query }, { "$options", "i" } } } };
            var artistResults = artistCollection.Find(artistfilter).ToList();
            foreach (BsonDocument result in songResults)
            {
                if (result != null)
                {
                    SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                    searchResultModel.SongGetModels.Add(res);
                }
            }
            foreach (BsonDocument result in albumResults)
            {
                if (result != null)
                {
                    AlbumGetModel res = BsonSerializer.Deserialize<AlbumGetModel>(result);
                    searchResultModel.AlbumGetModels.Add(res);
                }
            }
            foreach (BsonDocument result in artistResults)
            {
                if (result != null)
                {
                    ArtistGetInfoModel res = BsonSerializer.Deserialize<ArtistGetInfoModel>(result);
                    searchResultModel.ArtistGetInfoModels.Add(res);
                }
            }
            return searchResultModel;
        }
    }
}

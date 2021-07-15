using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models.Playlist;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Playlist
{
    public class PlaylistDataAccess
    {
        //COMMON METHODS
        public string UploadPlaylistCoverImage(IFormFile coverImage, string id)
        {
            string imageName = "playlistcovers/" + id;
            AzureBlobServiceProvider azureBlob = new AzureBlobServiceProvider();
            return azureBlob.UploadFileToBlob(imageName, coverImage);
        }
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
        }
        public bool IsListenerOrArtist(string id)
        {
            MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
            dbConnection.CreateQuery("SELECT * FROM users WHERE id='" + id + "' AND type='listener' OR type='artist'");
            MySqlDataReader reader = dbConnection.DoQuery();
            if (reader.Read())
            {
                return true;
            }
            dbConnection.Dispose();
            dbConnection = null;
            return false;
        }

        //INSERT ALBUM
        public bool CreatePlaylist(PlaylistGlobalModel playlistGlobalModel)
        {
            try
            {
                var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("playlists");
                var document = new BsonDocument
                    {
                        { "PlaylistId", playlistGlobalModel.PlaylistId },
                        { "PlaylistName", playlistGlobalModel.PlaylistName },
                        { "PlaylistImageUrl", playlistGlobalModel.PlaylistImageUrl },
                        { "CreatorId", playlistGlobalModel.CreatorId },
                        { "Songs", new BsonDocument{ } }
                    };
                collection.InsertOne(document);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

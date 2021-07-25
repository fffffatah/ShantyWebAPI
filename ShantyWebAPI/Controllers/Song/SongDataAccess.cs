using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Models.Song;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Song
{
    public class SongDataAccess
    {
        //COMMON METHODS
        public string UploadAudioFile(IFormFile audioFile, string id)
        {
            string songName = "songs/" + id;
            AzureBlobServiceProvider azureBlob = new AzureBlobServiceProvider();
            return azureBlob.UploadFileToBlob(songName, audioFile);
        }
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
        }
        public bool IsLabel(string id)
        {
            MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
            dbConnection.CreateQuery("SELECT * FROM users WHERE id='" + id + "' AND type='label'");
            MySqlDataReader reader = dbConnection.DoQuery();
            if (reader.Read())
            {
                return true;
            }
            dbConnection.Dispose();
            dbConnection = null;
            return false;
        }
        public bool IsLabelOrArtist(string id)
        {
            MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
            dbConnection.CreateQuery("SELECT * FROM users WHERE id='" + id + "' AND type='label' OR type='artist'");
            MySqlDataReader reader = dbConnection.DoQuery();
            if (reader.Read())
            {
                return true;
            }
            dbConnection.Dispose();
            dbConnection = null;
            return false;
        }

        //UPLOAD SONG
        public bool UploadSong(SongGlobalModel songGlobalModel)
        {
            try
            {
                var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
                var document = new BsonDocument
                    {
                        { "SongId", songGlobalModel.Id },
                        { "SongName", songGlobalModel.SongName },
                        { "SongFileUrl", songGlobalModel.SongFileUrl },
                        { "AlbumId", songGlobalModel.AlbumId },
                        { "ArtistName", songGlobalModel.ArtistName },
                        { "TimesStreamed", songGlobalModel.TimesStreamed},
                        { "Genre", songGlobalModel.Genre }
                    };
                collection.InsertOne(document);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //UPDATE SONG
        public bool UpdateSong(SongUpdateModel songUpdateModel)
        {
            if (songUpdateModel.SongFile != null)
            {
                songUpdateModel.SongFileUrl = UploadAudioFile(songUpdateModel.SongFile, songUpdateModel.SongId);
            }
            else
            {
                songUpdateModel.SongFileUrl = null;
            }
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var filter = Builders<BsonDocument>.Filter.Eq("SongId", songUpdateModel.SongId);
            var update = Builders<BsonDocument>.Update.Set("SongId", songUpdateModel.SongId);
            foreach (PropertyInfo prop in songUpdateModel.GetType().GetProperties())
            {
                var value = songUpdateModel.GetType().GetProperty(prop.Name).GetValue(songUpdateModel, null);
                if ((prop.Name != "SongId") && (prop.Name != "JwtToken") && (prop.Name != "SongFile"))
                {
                    if (value != null)
                    {
                        update = update.Set(prop.Name, value);
                    }
                }
            }
            if (collection.UpdateOne(filter, update).ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        //GET SONG BY ALBUM ID
        public SongGetModel GetSong(string id)
        {
            SongGetModel songGetModel = null;
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("SongId", id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                songGetModel = new SongGetModel();
                SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                songGetModel.SongId = res.SongId;
                songGetModel.SongName = res.SongName;
                songGetModel.ArtistName = res.ArtistName;
                songGetModel.AlbumId = res.AlbumId;
                songGetModel.Genre = res.Genre;
                songGetModel.SongFileUrl = res.SongFileUrl;
                songGetModel.TimesStreamed = res.TimesStreamed;
            }
            return songGetModel;
        }

        //GET ALBUM LIST FOR LABEL OR ARTIST
        public List<SongGetModel> GetSongsAlbum(string albumId)
        {
            List<SongGetModel> songGetModels = null;
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("AlbumId", albumId);
            var results = collection.Find(filter).ToList();
            foreach (BsonDocument result in results)
            {
                if (result != null)
                {
                    SongGetModel songGetModel = new SongGetModel();
                    SongGetModel res = BsonSerializer.Deserialize<SongGetModel>(result);
                    songGetModel.SongId = res.SongId;
                    songGetModel.SongName = res.SongName;
                    songGetModel.ArtistName = res.ArtistName;
                    songGetModel.AlbumId = res.AlbumId;
                    songGetModel.Genre = res.Genre;
                    songGetModel.SongFileUrl = res.SongFileUrl;
                    songGetModel.TimesStreamed = res.TimesStreamed;
                    songGetModels.Add(songGetModel);
                }
            }
            return songGetModels;
        }
        //DELETE ALBUM
        public bool DeleteSong(string songId)
        {
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("songs");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("SongId", songId);
            if (collection.DeleteOne(deleteFilter).DeletedCount > 0)
            {
                return true;
            }
            return false;
        }
    }
}

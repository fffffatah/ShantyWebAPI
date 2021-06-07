using Microsoft.AspNetCore.Http;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.Album;
using MongoDB.Bson;

namespace ShantyWebAPI.Controllers.Album
{
    public class AlbumDataAccess
    {
        MysqlConnectionProvider dbConnection;
        public AlbumDataAccess()
        {
            dbConnection = new MysqlConnectionProvider();
        }
        //COMMON METHODS
        public string UploadAlbumCoverImage(IFormFile coverImage, string id)
        {
            string imageName = "albumarts/" + id;
            AzureBlobServiceProvider azureBlob = new AzureBlobServiceProvider();
            return azureBlob.UploadFileToBlob(imageName, coverImage);
        }

        //INSERT ALBUM
        public bool CreateAlbum(AlbumGlobalModel albumGlobalModel)
        {
            try
            {
                var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("albums");
                var document = new BsonDocument
                    {
                        { "Id", albumGlobalModel.Id },
                        { "AlbumName", albumGlobalModel.AlbumName },
                        { "CoverImageUrl", albumGlobalModel.CoverImageUrl },
                        { "LabelId", albumGlobalModel.LabelId },
                        { "ArtistId", albumGlobalModel.ArtistId },
                        { "Year", albumGlobalModel.Year }
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using ShantyWebAPI.Models.User;
using ShantyWebAPI.Providers;

namespace ShantyWebAPI.Controllers.User
{
    public class UserDataAccess
    {
        MysqlConnectionProvider dbConnection;
        public UserDataAccess()
        {
            dbConnection = new MysqlConnectionProvider();
        }
        //COMMON METHODS
        public string UploadProfileImage(IFormFile profileImage, string id)
        {
            string imageName = "profileimages/" + id;
            AzureBlobServiceProvider azureBlob = new AzureBlobServiceProvider();
            return azureBlob.UploadFileToBlob(imageName, profileImage);
        }

        //LISTENER REGISTRATION
        public bool RegisterListener(ListenerGlobalModel listener)
        {
            bool InsertListenerMysql()
            {
                dbConnection.CreateQuery("INSERT INTO users(id, username, email, phone, pass, type, isemailverified) VALUES ('" + listener.Id + "','" + listener.Username + "','" + listener.Email + "','" + listener.Phone + "','" + listener.Pass + "','" + listener.Type + "','" + listener.IsEmailVerified + "')");
                if ((dbConnection.DoNoQuery()) < 1)
                {
                    dbConnection.Dispose();
                    return false;
                }
                dbConnection.Dispose();
                return true;
            }
            bool InsertListenerMongo()
            {
                var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("listeners");
                var document = new BsonDocument
                {
                    { "Id", listener.Id },
                    { "ProfileImageUrl", listener.ProfileImageUrl },
                    { "FirstName", listener.FirstName },
                    { "LastName", listener.LastName },
                    { "Dob", listener.Dob },
                    { "Region", listener.Region },
                    { "IsSubscriber", listener.IsSubscriber }
                };
                collection.InsertOne(document);
                return true;
            }
            if (InsertListenerMysql() && InsertListenerMongo())
            {
                return true;
            }
            return false;
        }
    }
}

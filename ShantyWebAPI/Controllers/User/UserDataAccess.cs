using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MySql.Data.MySqlClient;
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
        public string UploadLabelIcon(IFormFile labelIcon, string id)
        {
            string imageName = "labelicons/" + id;
            AzureBlobServiceProvider azureBlob = new AzureBlobServiceProvider();
            return azureBlob.UploadFileToBlob(imageName, labelIcon);
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
                try
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
                catch (Exception)
                {
                    return false;
                }
            }
            return (InsertListenerMysql() && InsertListenerMongo());
        }

        //LABEL REGISTRTION
        public bool RegisterLabel(LabelGlobalModel label)
        {
            bool InsertLabelMysql()
            {
                dbConnection.CreateQuery("INSERT INTO users(id, username, email, phone, pass, type, isemailverified) VALUES ('" + label.Id + "','" + label.Username + "','" + label.Email + "','" + label.Phone + "','" + label.Pass + "','" + label.Type + "','" + label.IsEmailVerified + "')");
                if ((dbConnection.DoNoQuery()) < 1)
                {
                    dbConnection.Dispose();
                    return false;
                }
                dbConnection.Dispose();
                return true;
            }
            bool InsertLabelMongo()
            {
                try
                {
                    var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("labels");
                    var document = new BsonDocument
                    {
                        { "Id", label.Id },
                        { "LabelIconUrl", label.LabelIconUrl },
                        { "LabelName", label.LabelName },
                        { "EstDate", label.EstDate },
                        { "Region", label.Region },
                        { "IsVerified", label.IsVerified }
                    };
                    collection.InsertOne(document);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return (InsertLabelMysql() && InsertLabelMongo());
        }

        //ARTIST REGISTRATION
        public bool RegisterArtist(ArtistGlobalModel artist)
        {
            bool InsertArtistMysql()
            {
                dbConnection.CreateQuery("INSERT INTO users(id, username, email, phone, pass, type, isemailverified) VALUES ('" + artist.Id + "','" + artist.Username + "','" + artist.Email + "','" + artist.Phone + "','" + artist.Pass + "','" + artist.Type + "','" + artist.IsEmailVerified + "')");
                if ((dbConnection.DoNoQuery()) < 1)
                {
                    dbConnection.Dispose();
                    return false;
                }
                dbConnection.Dispose();
                return true;
            }
            bool InsertArtistMongo()
            {
                try
                {
                    var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("artists");
                    var document = new BsonDocument
                    {
                        { "Id", artist.Id },
                        { "ProfileImageUrl", artist.ProfileImageUrl },
                        { "FirstName", artist.FirstName },
                        { "LastName", artist.LastName },
                        { "Dob", artist.Dob },
                        { "Region", artist.Region },
                        { "LabelId", artist.LabelId },
                        { "IsVerified", artist.IsVerified }
                    };
                    collection.InsertOne(document);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return (InsertArtistMysql() && InsertArtistMongo());
        }

        //USER LOGIN
        public string LoginUser(string email, string pass)
        {
            dbConnection.CreateQuery("SELECT id,pass,isemailverified FROM users WHERE email='" + email + "'");
            UserLoginResponseModel userLoginResponseModel = null;
            string passFromDb = "";
            string isEmailVerified = "";
            MySqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                userLoginResponseModel = new UserLoginResponseModel();
                userLoginResponseModel.Id = reader["id"].ToString();
                passFromDb = reader["pass"].ToString();
                isEmailVerified = reader["isemailverified"].ToString();
            }
            dbConnection.Dispose();
            dbConnection = null;
            if (userLoginResponseModel != null)
            {
                if (isEmailVerified == "true")
                {
                    if (BCrypt.Net.BCrypt.Verify(pass, passFromDb))
                    {
                        return new JwtAuthenticationProvider().GenerateJsonWebToken(userLoginResponseModel);
                    }
                }
                else
                {
                    return "Email Not Verified";
                }
            }
            return "";
        }

        //EMAIL VERIFICATION
        public void SendVerificationEmail(string name, string email, string id)
        {
            string url = "https://wwww.yourfrontendurl.com/email/verify?id=" + id; //YOUR FRONTEND URL, MAKE SURE TO PASS THE API SUBSCRIPTION KEY AS HEADER AS WELL
            SendgridEmailProvider sendgridEmailProvider = new SendgridEmailProvider();
            sendgridEmailProvider.Send("no-reply@shanty.com", "Shanty", email, name, "Shanty - Verification", "Confirmation Email for Your Shanty Account", "<strong>Confirm Your Email Address: <u><a href=" + url + " target=\"_blank\">Click Here</a></u></strong>");
        }
        public bool VerifyEmail(string id)
        {
            dbConnection.CreateQuery("UPDATE users SET isemailverified='true' WHERE id='" + id+"'");
            if ((dbConnection.DoNoQuery()) < 1)
            {
                dbConnection.Dispose();
                return false;
            }
            dbConnection.Dispose();
            return true;
        }
    }
}

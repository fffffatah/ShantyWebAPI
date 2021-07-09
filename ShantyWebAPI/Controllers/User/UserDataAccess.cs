using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Models.User;
using ShantyWebAPI.Providers;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

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
        public string JwtTokenValidation(string jwt)
        {
            return new JwtAuthenticationProvider().ValidateToken(jwt);
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

        //RESET OR CHANGE PASSWORD
        public string SendOtpForPassReset(string otp, string email)
        {
            bool IsEmailTaken(string email)
            {
                MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
                dbConnection.CreateQuery("SELECT COUNT(*) AS \"COUNTER\" FROM users WHERE email='" + email + "'");
                MySqlDataReader reader = dbConnection.DoQuery();
                string counter = "";
                while (reader.Read())
                {
                    counter = reader["COUNTER"].ToString();
                }
                dbConnection.Dispose();
                return !counter.Equals("0");
            }
            if (!IsEmailTaken(email))
                {
                return "Email Not Found";
            }
            SendgridEmailProvider sendgridEmailProvider = new SendgridEmailProvider();
            sendgridEmailProvider.Send("no-reply@shanty.com", "Shanty", email, "User", "Shanty - OTP", "OTP for Password Reset", "<strong>OTP: " + otp + "</strong>");
            dbConnection.CreateQuery("SELECT id,isemailverified FROM users WHERE email='" + email + "'");
            UserLoginResponseModel userLoginResponseModel = null;
            string isEmailVerified = "";
            MySqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                userLoginResponseModel = new UserLoginResponseModel();
                userLoginResponseModel.Id = reader["id"].ToString();
                isEmailVerified = reader["isemailverified"].ToString();
            }
            dbConnection.Dispose();
            dbConnection = null;
            if (userLoginResponseModel != null)
            {
                if (isEmailVerified == "false")
                {
                    return "Email Not Verified";
                }
                return new JwtAuthenticationProvider().GenerateJsonWebToken(userLoginResponseModel);
            }
            return "";
        }

        public bool ResetChangePassword(string id, string pass)
        {
            MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
            dbConnection.CreateQuery("UPDATE users SET pass='" + pass + "' WHERE id='" + id + "'");
            if ((dbConnection.DoNoQuery()) < 1)
            {
                dbConnection.Dispose();
                return false;
            }
            dbConnection.Dispose();
            return true;
        }
       
        //UPDATE USER DATA
        public bool UpdateArtist(ArtistUpdateModel artistUpdateModel)
        {
            //TODO
            /*{ "ProfileImageUrl", artist.ProfileImageUrl },
                        { "FirstName", artist.FirstName },
                        { "LastName", artist.LastName },
                        { "Dob", artist.Dob },*/
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("artists");
            var filter = Builders<BsonDocument>.Filter.Eq("Id", artistUpdateModel.Id);
            var update = Builders<BsonDocument>.Update.Set("ProfileImageUrl", artistUpdateModel.ProfileImageUrl);
            return false;
        }
        public bool UpdateListener(ListenerUpdateModel listenerUpdateModel)
        {
            //todo
            return false;
        }
        public bool UpdateLabel(LabelUpdateModel labelUpdateModel)
        {
            //todo
            return false;
        }

        //GET USER DATA
        public ArtistGetInfoModel GetArtistInfo(string Id)
        {
            dbConnection.CreateQuery("SELECT username,email,phone FROM users WHERE id='"+Id+"'");
            ArtistGetInfoModel artistGetInfoModel = null;
            MySqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                artistGetInfoModel = new ArtistGetInfoModel();
                artistGetInfoModel.Username = reader["username"].ToString();
                artistGetInfoModel.Email = reader["email"].ToString();
                artistGetInfoModel.Phone = reader["phone"].ToString();
            }
            dbConnection.Dispose();
            dbConnection = null;
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("artists");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Id", Id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                ArtistGetInfoModel res = BsonSerializer.Deserialize<ArtistGetInfoModel>(result);
                artistGetInfoModel.ProfileImageUrl = res.ProfileImageUrl;
                artistGetInfoModel.FirstName = res.FirstName;
                artistGetInfoModel.LastName = res.LastName;
                artistGetInfoModel.Dob = res.Dob;
                artistGetInfoModel.Region = res.Region;
                artistGetInfoModel.IsVerified = res.IsVerified;
                artistGetInfoModel.LabelId = res.LabelId;
            }
            return artistGetInfoModel;
        }
        public ListenerGetInfoModel GetListenerInfo(string Id)
        {
            dbConnection.CreateQuery("SELECT username,email,phone FROM users WHERE id='" + Id + "'");
            ListenerGetInfoModel listenerGetInfoModel = null;
            MySqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                listenerGetInfoModel = new ListenerGetInfoModel();
                listenerGetInfoModel.Username = reader["username"].ToString();
                listenerGetInfoModel.Email = reader["email"].ToString();
                listenerGetInfoModel.Phone = reader["phone"].ToString();
            }
            dbConnection.Dispose();
            dbConnection = null;
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("listeners");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Id", Id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                ListenerGetInfoModel res = BsonSerializer.Deserialize<ListenerGetInfoModel>(result);
                listenerGetInfoModel.ProfileImageUrl = res.ProfileImageUrl;
                listenerGetInfoModel.FirstName = res.FirstName;
                listenerGetInfoModel.LastName = res.LastName;
                listenerGetInfoModel.Dob = res.Dob;
                listenerGetInfoModel.Region = res.Region;
                listenerGetInfoModel.IsSubscriber = res.IsSubscriber;
            }
            return listenerGetInfoModel;
        }
        public LabelGetInfoModel GetLabelInfo(string Id)
        {
            dbConnection.CreateQuery("SELECT username,email,phone FROM users WHERE id='" + Id + "'");
            LabelGetInfoModel labelGetInfoModel = null;
            MySqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                labelGetInfoModel = new LabelGetInfoModel();
                labelGetInfoModel.Username = reader["username"].ToString();
                labelGetInfoModel.Email = reader["email"].ToString();
                labelGetInfoModel.Phone = reader["phone"].ToString();
            }
            dbConnection.Dispose();
            dbConnection = null;
            var collection = new MongodbConnectionProvider().GeShantyDatabase().GetCollection<BsonDocument>("labels");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Id", Id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                LabelGetInfoModel res = BsonSerializer.Deserialize<LabelGetInfoModel>(result);
                labelGetInfoModel.LabelIconUrl = res.LabelIconUrl;
                labelGetInfoModel.LabelName = res.LabelName;
                labelGetInfoModel.EstDate = res.EstDate;
                labelGetInfoModel.IsVerified = res.IsVerified;
                labelGetInfoModel.Region = res.Region;
            }
            return labelGetInfoModel;
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
                if (BCrypt.Net.BCrypt.Verify(pass, passFromDb))
                {
                    if (isEmailVerified == "false")
                    {
                        return "Email Not Verified";
                    }
                    return new JwtAuthenticationProvider().GenerateJsonWebToken(userLoginResponseModel);
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        //EMAIL VERIFICATION
        public void SendVerificationEmail(string name, string email, string id)
        {
            string url = Environment.GetEnvironmentVariable("EMAIL_VERIFICATION_URL") + id; //YOUR FRONTEND URL, MAKE SURE TO PASS THE API SUBSCRIPTION KEY AS HEADER AS WELL
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

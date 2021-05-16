using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ShantyWebAPI.Providers
{
    public class MongodbConnectionProvider
    {
        MongoClient dbClient;

        public MongodbConnectionProvider()
        {
            dbClient = new MongoClient(Environment.GetEnvironmentVariable("CUSTOMMONGO_CONN_STRING"));
        }
        public IMongoDatabase GeShantyDatabase()
        {
            return dbClient.GetDatabase("shanty");
        }
    }
}

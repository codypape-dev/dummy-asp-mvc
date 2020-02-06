using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace dummy_asp_mvc.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        public IMongoDatabase _database;
        public MongoContext()        //constructor   
        {
            // Reading credentials from Web.config file   
            var MongoConnString = ConfigurationManager.AppSettings["MongoConnString"];
            var MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

            _client = new MongoClient(MongoConnString);
            _database = _client.GetDatabase(MongoDatabaseName);
        }
    }
}
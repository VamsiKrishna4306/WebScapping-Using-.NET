using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Configuration;

namespace MongoDBCRUDOperations.App_Start
{
    public class MongoDBContext
    {
        MongoClient client;
        public IMongoDatabase database;

        public MongoDBContext()
        {
            var mongoClient = new MongoClient("mongodb://Vamsi:Vamsi%404306@cluster0-shard-00-00-03y41.mongodb.net:27017,cluster0-shard-00-01-03y41.mongodb.net:27017,cluster0-shard-00-02-03y41.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true&w=majority");
            database = mongoClient.GetDatabase("first_test");
        }

    }
}
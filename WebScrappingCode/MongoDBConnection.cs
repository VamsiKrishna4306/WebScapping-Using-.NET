using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Canada_Job_Bank_Web_Scrapping1
{
    class MongoDBCOnnection
    {

        public void InsertData()
        {
            // connection string to connect to Mongodb cloud atlas
            var connectionString = "mongodb://Vamsi:Vamsi%404306@cluster0-shard-00-00-03y41.mongodb.net:27017,cluster0-shard-00-01-03y41.mongodb.net:27017,cluster0-shard-00-02-03y41.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true&w=majority";
            // creating new client connection to MOngo db using connection string
            var client = new MongoClient(connectionString);
            // Accessing our database
            var db = client.GetDatabase("first_test");
            // Retreiving our collection
            var col = db.GetCollection<BsonDocument>("Assignment_Test3");
            // Reading json data to be inserted
            var reader = new StreamReader(@"D:/Bdat_Course_Material/Semister 2/Social data mining techniques/C#/C#_Canada_job_listings.json");
            string line;
            var sb = new StringBuilder();
            // reading data till end of file
            while ((line = reader.ReadLine()) != null)
            {
                sb.Append(line);
            }
            // converting the data to array
            var arr = JArray.Parse(sb.ToString());
            foreach (JObject o in arr)
            {
                // parsing data as Bson document
                var d = BsonDocument.Parse(o.ToString());
                // inserting data 
                col.InsertOne(d);
            }



        }


    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;


namespace MongoDBTrial
{
    class Program
    {
        static void Main(string[] args)
        {
            var mongo = new MongoClient().GetServer();
            var db = mongo.GetDatabase("logs");
            var cred_collection = db.GetCollection("logs");

            cred_collection.RemoveAll();

            foreach (IndexInfo index in cred_collection.GetIndexes())
            {
                Console.WriteLine(String.Format("Key:{0}    Name:{1}",index.Key,index.Name));
            }
            Stopwatch watch = new Stopwatch();

            watch.Start();

            for (int i = 0; i < 1000000; i++)
            {
                var creds = new CredentialSet
                    {
                        Title = String.Format("Joe's creds {0}", i),
                        LastUpdate = DateTime.Now,
                        Notes = String.Format("This is my notes for {0}", i)
                    };

                cred_collection.Save(creds);


            }
            watch.Stop();

            Console.WriteLine(String.Format("Elapsed Time: {0}", watch.Elapsed));

            Console.Read();
        }
    }

    public class CredentialSet
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string WebSite { get; set; }
        public string Notes { get; set; }
        public int Owner { get; set; }
        public DateTime LastUpdate { get; set; }
        public int SecretNumber { get;set; }
    }
}

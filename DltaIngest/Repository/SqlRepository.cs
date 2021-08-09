using DltaIngest.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Collections.Generic;

namespace DltaIngest.Repository
{
    public class SqlRepository : IRepository<Azure>
    {
        private readonly IMongoDatabase _db;

        public SqlRepository(MongoDBInit mongoDBInit)
        {
            var client = new MongoClient(mongoDBInit.ConnectionString);
            _db = client.GetDatabase(mongoDBInit.DatabaseName);

            Log.Information("Conneting to database {0}", mongoDBInit.DatabaseName);
        }

        public T FindRecoredByID<T>(string table, string FullyQualifiedDomainName)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("FullyQualifiedDomainName", FullyQualifiedDomainName);
            return collection.Find(filter).First();
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = _db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public void InsertRecords<T>(string table, List<T> records)
        {
            var collection = _db.GetCollection<T>(table);
            collection.InsertMany(records);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = _db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }
}

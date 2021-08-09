using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using DltaIngest.Repository;
using DltaIngest.Models;
using Microsoft.Extensions.Configuration;

namespace DltaIngest.Repository
{
    public class MongoDBInit : DatabaseSettings
    {
        private IConfiguration _config;


        public MongoDBInit(IConfiguration config)
        {
            _config = config;
            if (_config != null)
            {
                this.ConnectionString = _config.GetValue<string>("DatabaseSettings:ConnectionString");
                this.DatabaseName = _config.GetValue<string>("DatabaseSettings:DatabaseName");
            }
        }

    }
}

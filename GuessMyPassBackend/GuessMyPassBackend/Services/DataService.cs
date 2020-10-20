﻿using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core;
using GuessMyPassBackend.Models;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GuessMyPassBackend.Services
{
    public class DataService
    {
        private readonly IMongoDatabase _database = null;

        private string data = "data";
        public DataService(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
            }

        }

        public List<Data> GetAllData(string token)
        {

            string email = DecodeJwtEmail(token);

            return _database.GetCollection<Data>("data").Find<Data>(data => data.Owner == email).ToList();
        }

        public Data CreateData(Data data, string token)
        {

            string decoded = DecodeJwtEmail(token);

            data.Owner = decoded;

            _database.GetCollection<Data>("data").InsertOne(data);

            return data;
        }

        public async Task<Data> UpdateData(Data data)
        {
            ObjectId line = ObjectId.Parse(data.id);
            FilterDefinition<Data> filter = Builders<Data>.Filter.Eq<Object>("_id", ObjectId.Parse(data.id));
            UpdateDefinition<Data> update = Builders<Data>.Update.Set("name", data.Name);
            return await _database.GetCollection<Data>("data").FindOneAndUpdateAsync<Data>(filter, update);
            // ToDo: Return updated data
        }

        private string DecodeJwtEmail(string tokenString)
        {

            try
            {
                JwtSecurityToken token = new JwtSecurityToken(tokenString);

                string email = token.Claims.First(c => c.Type == "email").Value;

                return email;
            } catch (Exception) {

                return "";

            }

            
        }

    }
}
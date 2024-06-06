using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.CasheServices
{
    public class ResponseCasheServices : IResponseCasheServices
    {
        private readonly IDatabaseAsync _database; 
        public ResponseCasheServices(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }
        public async Task CasheResponseAsync(string key, object Response, TimeSpan TimeToLive)
        {
            if (Response is null) return;
            
            var serializeOption = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            
            var SerializedResponse = JsonSerializer.Serialize(Response , serializeOption);

            await _database.StringSetAsync(key, SerializedResponse , TimeToLive);
                
            
        }

        public async Task<string?> GetCashedResponseAsync(string key)
        {
            var response = await _database.StringGetAsync(key);

            if (response.IsNullOrEmpty) return null;
            return response;
 
        }
    }
}

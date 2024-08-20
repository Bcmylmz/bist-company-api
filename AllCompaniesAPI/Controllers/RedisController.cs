using AllCompaniesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace AllCompaniesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpPost]
        public async Task<IActionResult> SetData(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
            return Ok("Data set in Redis");
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (string.IsNullOrEmpty(value))
            {
                Company jsonData =  JsonConvert.DeserializeObject<Company>(value);
                return Ok(jsonData);
            }
                return Ok(value.ToString());
            }
        
    }
}

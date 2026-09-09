using Microsoft.EntityFrameworkCore.Storage;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Services.BasketService;
using ShopHub.Domain.Entities.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.Repositories.BasketRepo
{
    public class BasketRepository : IBasketRepository
    {

        private readonly StackExchange.Redis.IDatabase _redis;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _redis.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _redis.StringGetAsync(basketId);

            if (basket.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);

            var created = await _redis.StringSetAsync(
                basket.Id,
                serializedBasket,
                TimeSpan.FromDays(30));

            if (!created)
                return null;

            return basket;
        }
    }


}

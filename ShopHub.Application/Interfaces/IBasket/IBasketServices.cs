using ShopHub.Application.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.IBasket
{
    public interface IBasketServices
    {
        Task<CustomerBasketDto> GetOrCreateBasketAsync(string basketId);
        Task<CustomerBasketDto> AddItemAsync(string basketId, BasketItemDto item, int quantity);
        Task<CustomerBasketDto> UpdateItemQuantityAsync(string basketId, int itemId, int quantity);
        Task RemoveItemAsync(string basketId, int itemId);
        Task ClearBasketAsync(string basketId);
    }
}

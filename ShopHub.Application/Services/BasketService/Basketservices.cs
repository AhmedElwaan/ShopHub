using AutoMapper;
using ShopHub.Application.DTOs.BasketDTOs;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.IBasket;
using ShopHub.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Services.BasketService
{
    public class Basketservices : IBasketServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public Basketservices(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        public async Task<CustomerBasketDto> AddItemAsync(string basketId, BasketItemDto item, int quantity)
        {
            ValidateBasketId(basketId);
            ValidateItem(item);
            ValidateQuantity(quantity);

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
            {
                basket = new CustomerBasket
                {
                    Id = basketId,
                    Items = new List<BasketItem>()
                };
            }

            var existingItem = basket.Items
                .FirstOrDefault(x => x.ProductId == item.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    PictureUrl = item.PictureUrl,
                    Quantity = quantity
                });
            }

            await _basketRepository.UpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task ClearBasketAsync(string basketId)
        {
            ValidateBasketId(basketId);

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
                return;

            basket.Items.Clear();

            await _basketRepository.UpdateBasketAsync(basket);
        }

        public async Task<CustomerBasketDto> GetOrCreateBasketAsync(string basketId)
        {
            ValidateBasketId(basketId);

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
            {
                basket = new CustomerBasket
                {
                    Id = basketId,
                    Items = new List<BasketItem>()
                };

                await _basketRepository.UpdateBasketAsync(basket);
            }

            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task RemoveItemAsync(string basketId, int itemId)
        {
            ValidateBasketId(basketId);

            if (itemId <= 0)
                throw new ArgumentException(
                    "Invalid product id.",
                    nameof(itemId));

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
                throw new KeyNotFoundException("Basket not found.");

            var item = basket.Items
                .FirstOrDefault(x => x.ProductId == itemId);

            if (item == null)
                throw new KeyNotFoundException(
                    "Item not found in basket.");

            basket.Items.Remove(item);

            await _basketRepository.UpdateBasketAsync(basket);
        }

        public async Task<CustomerBasketDto> UpdateItemQuantityAsync(string basketId, int itemId, int quantity)
        {
            ValidateBasketId(basketId);

            if (itemId <= 0)
                throw new ArgumentException(
                    "Invalid product id.",
                    nameof(itemId));

            ValidateQuantity(quantity);

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
                throw new KeyNotFoundException("Basket not found.");

            var item = basket.Items
                .FirstOrDefault(x => x.ProductId == itemId);

            if (item == null)
                throw new KeyNotFoundException("Item not found in basket.");

            item.Quantity = quantity;

            await _basketRepository.UpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasketDto>(basket);
        }


        // ---- private validation helpers ----
        private static void ValidateBasketId(string basketId)
        {
            if (string.IsNullOrWhiteSpace(basketId))
                throw new ArgumentException("Basket id is required.", nameof(basketId));
        }

        private static void ValidateItem(BasketItemDto item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id <= 0)
                throw new ArgumentException("Invalid product id.", nameof(item));

            if (item.Price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(item));
        }

        private static void ValidateQuantity(int quantity)
        {
            if (quantity is <= 0 or > 100)
                throw new ArgumentException("Quantity must be between 1 and 100.", nameof(quantity));
        }
    }
}

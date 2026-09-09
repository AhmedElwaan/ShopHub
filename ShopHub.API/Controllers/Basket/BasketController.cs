using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.BasketDTOs;
using ShopHub.Application.Interfaces.IBasket;
using System.Security.Claims;

namespace ShopHub.API.Controllers.Basket
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServices _basketService;

        public BasketController(IBasketServices basketService)
        {
            _basketService = basketService;
        }

        // GET api/basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket()
        {
            var basketId = GetBasketId();
            var basket = await _basketService.GetOrCreateBasketAsync(basketId);
            return Ok(basket);
        }

        // POST api/basket/items
        [HttpPost("items")]
        public async Task<ActionResult<CustomerBasketDto>> AddItem(BasketItemDto item, int quantity)
        {
            var basketId = GetBasketId();
            var basket = await _basketService.AddItemAsync(basketId, item, quantity);
            return Ok(basket);
        }

        // PUT api/basket/items/{itemId}
        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CustomerBasketDto>> UpdateItemQuantity(int itemId, int quantity)
        {
            var basketId = GetBasketId();
            var basket = await _basketService.UpdateItemQuantityAsync(basketId, itemId, quantity);
            return Ok(basket);
        }

        // DELETE api/basket/items/{itemId}
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var basketId = GetBasketId();
            await _basketService.RemoveItemAsync(basketId, itemId);
            return NoContent();
        }

        // DELETE api/basket
        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            var basketId = GetBasketId();
            await _basketService.ClearBasketAsync(basketId);
            return NoContent();
        }

        // ---- helper ----

        private string GetBasketId()
        {
            // Pull the user id from the JWT claims — never trust a client-supplied basketId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }
    }
}

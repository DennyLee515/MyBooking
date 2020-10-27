using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MyBooking.API.Dtos;
using MyBooking.API.Models;
using MyBooking.API.Services;
using MyBooking.API.Utils;

namespace MyBooking.API.Controllers
{
    [Route("api/shoppingCart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetShoppingCart()
        {
            // 1. Get current user
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Get shopping cart using userId
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody] ShoppingCartItemAdditionDto shoppingCartItemAdditionDto)
        {
            // 1. Get current user
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Get shopping cart using userId
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            // 3. Create LineItem
            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(shoppingCartItemAdditionDto.TouristRouteId);

            if (touristRoute == null)
            {
                return NotFound("No such route found");
            }

            var lineItem = new LineItem()
            {
                ShoppingCartId = shoppingCart.Id,
                TouristRouteId = touristRoute.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.Discount
            };

            // 4. Save LineItem to database
            await _touristRouteRepository.AddShoppingCartItemAsync(lineItem);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            // 1. Get LineItem  
            var lineItem = await _touristRouteRepository.GetShoppingCartItemByItemId(itemId);
            if (lineItem == null)
            {
                return NotFound("Cannot find this item in shopping cart");
            }
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("items/({itemIDs})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveShoppingCartItems([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIDs)
        {
            var lineItems = await _touristRouteRepository.GetShoppingCartItemsByIdsAsync(itemIDs);

            _touristRouteRepository.DeleteShoppingCartItems(lineItems);

            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Checkout()
        {
            // 1. Get current user
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. Get shopping cart using userId
            var shoppingCart = await _touristRouteRepository.GetShoppingCartByUserIdAsync(userId);

            // 3. Create Order
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart.ShoppingCartItems,
                CreateDateUTC = DateTime.UtcNow,
            };
            shoppingCart.ShoppingCartItems = null;

            // 4. Save data
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();

            // 5. Return
            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}

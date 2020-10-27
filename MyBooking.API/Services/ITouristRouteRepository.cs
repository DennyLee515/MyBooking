using MyBooking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Services
{
    public interface ITouristRouteRepository
    {
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue);
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePic>> GetPicsByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePic> GetPicAsync(int pictureId);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDsAsync(IEnumerable<Guid> ids);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePic(Guid touristRouteId, TouristRoutePic touristRoutePic);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);
        void DeleteTouristRoutePicture(TouristRoutePic touristRoutePic);
        Task AddShoppingCartItemAsync(LineItem lineItem);
        Task CreateShoppingCartAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId);
        Task<LineItem> GetShoppingCartItemByItemId(int lineItemId);
        void DeleteShoppingCartItem(LineItem lineItem);
        Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdsAsync(IEnumerable<int> ids);
        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);
        Task AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task<Order> GetOrderByOrderId(Guid orderId);
        Task<bool> SaveAsync();
    }
}

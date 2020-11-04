using Microsoft.EntityFrameworkCore;
using MyBooking.API.Database;
using MyBooking.API.Models;
using MyBooking.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;
        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePic(Guid touristRouteId, TouristRoutePic touristRoutePic)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));

            }

            if(touristRoutePic == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePic));
            }

            touristRoutePic.TouristRouteId = touristRouteId;
            _context.TouristRoutePics.Add(touristRoutePic);
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePic touristRoutePic)
        {
            _context.TouristRoutePics.Remove(touristRoutePic);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public async Task<Order> GetOrderByOrderId(Guid orderId)
        {
            return await _context.Orders
                .Include(order => order.OrderItems).ThenInclude(orderItem => orderItem.TouristRoute)
                .Where(order => order.Id == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginationList<Order>> GetOrdersByUserId(string userId,int pageSize, int pageNumber)
        {
            IQueryable<Order> result = _context.Orders.Where(order => order.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber,pageSize,result);
        }

        public async Task<TouristRoutePic> GetPicAsync(int pictureId)
        {
            return await _context.TouristRoutePics.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoutePic>> GetPicsByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePics.Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(shopingCart => shopingCart.User)
                .Include(shopingCart => shopingCart.ShoppingCartItems).ThenInclude(lineItem => lineItem.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems.Where(lineItem => lineItem.Id == lineItemId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.LineItems.Where(lineItem => ids.Contains(lineItem.Id)).ToListAsync();
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePics).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageSize, int pageNumber)
        {
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePics);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if (ratingValue >= 0)
            {
                result = ratingOperator switch
                {
                    "greaterThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }

            return await PaginationList<TouristRoute>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDsAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }
    }
}

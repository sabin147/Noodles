using Noodles.Models;

namespace Noodles.Managers
{
    public class OrderItemManager
    {
        private readonly NoodlesDBContext _context;
        public OrderItemManager(NoodlesDBContext context)
        {
            _context = context;
        }
        public List<OrderItem> GetAll()
        {
            return _context.OrderItems.ToList();
        }
        public OrderItem GetById(int id)
        {
            return _context.OrderItems.Find(id);
        }
        public OrderItem Delete(int id)
        {
            var item = _context.OrderItems.Find(id);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
        public List<OrderItem> GetBasketHistoryForUser(int orderId)
        {
            // Retrieve order history for the specified user
            return _context.OrderItems
                .Where(o => o.Order.UserId == orderId)
                .ToList();
        }
    }
}

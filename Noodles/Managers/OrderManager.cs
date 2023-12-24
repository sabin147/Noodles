   using Noodles.Models;

namespace Noodles.Managers
{
    public class OrderManager
    {
        private readonly NoodlesDBContext _context;
        public OrderManager(NoodlesDBContext context)
        {
            _context = context;
        }
       
            public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }
        public Order GetById(int id)
        {
            return _context.Orders.Find(id);
        }
        public Order Add(Order item)
        {
            _context.Orders.Add(item);
            _context.SaveChanges();
            return item;
        }
        public Order Update(int id, Order updates)
        {
            var item = _context.Orders.Find(id);
            if (item != null)
            {
                item.OrderId = updates.OrderId;
                item.UserId = updates.UserId;
                item.OrderDate = updates.OrderDate;
                item.TotalAmount= updates.TotalAmount;
                _context.SaveChanges();
            }
            return item;
        }
        public Order Delete(int id)
        {
            var item = _context.Orders.Find(id);
            if (item != null)
            {
                _context.Orders.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
        //public async Task<Order> CreateOrder(int userId, List<OrderItemDTO> orderItems)
        //{
        //    decimal totalAmount = 0;

        //    var order = new Order
        //    {
        //        UserId = userId,
        //        OrderDate = DateTime.Now,
        //        OrderItems = new List<OrderItem>(),        
        //    };

        //    foreach (var item in orderItems)
        //    {
        //        var foodItem = await _context.FoodItems.FindAsync(item.FoodItemId);
        //        if (foodItem == null)
        //        {
        //            throw new ArgumentException("Invalid food item ID: " + item.FoodItemId);
        //        }

        //        var orderItem = new OrderItem
        //        {
        //            FoodItemId = item.FoodItemId,
        //            Quantity = item.Quantity,
        //            Subtotal = foodItem.Price * item.Quantity
        //        };

        //        totalAmount += orderItem.Subtotal;
        //        order.OrderItems.Add(orderItem);
        //    }

        //    order.TotalAmount = totalAmount;

        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return order;
        //}
        public async Task<Order> CreateOrder(int userId, List<OrderItemDTO> orderItems, decimal subscriptionDiscountPercentage)
        {
            decimal totalAmount = 0;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>(),
            };

            foreach (var item in orderItems)
            {
                var foodItem = await _context.FoodItems.FindAsync(item.FoodItemId);
                if (foodItem == null)
                {
                    throw new ArgumentException("Invalid food item ID: " + item.FoodItemId);
                }

                var orderItem = new OrderItem
                {
                    FoodItemId = item.FoodItemId,
                    Quantity = item.Quantity,
                    Subtotal = foodItem.Price * item.Quantity
                };

                totalAmount += orderItem.Subtotal;
                order.OrderItems.Add(orderItem);
            }

            // Apply subscription discount
            totalAmount *= (1 - subscriptionDiscountPercentage / 100);

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}

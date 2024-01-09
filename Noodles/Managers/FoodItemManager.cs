using Noodles.Models;

namespace Noodles.Managers
{
    public class FoodItemManager
    {
        private readonly NoodlesDBContext _context;
        public FoodItemManager(NoodlesDBContext context)
        {
            _context = context;
        }

        public List<FoodItem> GetAll()
        {
            return _context.FoodItems.ToList();
        }
        public FoodItem GetById(int id)
        {
            return _context.FoodItems.Find(id);
        }
        public FoodItem Add(FoodItem item)
        {
            _context.FoodItems.Add(item);
            _context.SaveChanges();
            return item;
        }
        public FoodItem Update(int id, FoodItem updates)
        {
            var item = _context.FoodItems.Find(id);
            if (item != null)
            {
                item.Name= updates.Name;
                item.Description= updates.Description;
                item.Price= updates.Price;
                _context.SaveChanges();
            }
            return item;
        }
        public FoodItem Delete(int id)
        {
            var item = _context.FoodItems.Find(id);
            if (item != null)
            {
                _context.FoodItems.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
        public List<FoodItem> GetFoodHistoryForUser(int userId)
        {
            return _context.FoodItems
                .Where(foodItem => foodItem.OrderItems.Any(orderItem => orderItem.Order.UserId == userId))
                .OrderByDescending(foodItem => foodItem.OrderItems.OrderByDescending(orderItem => orderItem.Order.OrderDate).FirstOrDefault().Order.OrderDate)
                .ToList();
        }

    }
}

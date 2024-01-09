using Microsoft.EntityFrameworkCore;
using Noodles.Models;

namespace Noodles.Managers
{
    public class SubscriptionManager
    {
        private readonly NoodlesDBContext _context;

        public SubscriptionManager(NoodlesDBContext dbContext)
        {
            _context = dbContext;
        }

        public List<Subscription> GetAll()
        {
            return _context.Subscriptions.ToList();
        }
        public Subscription GetById(int id)
        {
            return _context.Subscriptions.Find(id);
        }
        public Subscription Add(Subscription item)
        {
            _context.Subscriptions.Add(item);
            _context.SaveChanges();
            return item;
        }
        public Subscription Update(int id, Subscription updates)
        {
            var item = _context.Subscriptions.Find(id);
            if (item != null)
            {
                item.Name = updates.Name;
                item.Price = updates.Price;
                item.DiscountPercentage = updates.DiscountPercentage;
                _context.SaveChanges();
            }
            return item;
        }
        public Subscription Delete(int id)
        {
            var item = _context.Subscriptions.Find(id);
            if (item != null)
            {
                _context.Subscriptions.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
        public List<Subscription> GetSubscriptionHistoryForUser(int userId)
        {
            return _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Subscription)
                .ToList();
        }
    }

}

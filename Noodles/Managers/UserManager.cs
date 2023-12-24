using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noodles.Models;


namespace Noodles.Managers
{
    public class UserManager
    {
        
        private readonly NoodlesDBContext _context;
 

        public UserManager(NoodlesDBContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.UserId == id);
        }
        public User Add(User item)
        {
            try
            {
                _context.Users.Add(item);
                _context.SaveChanges();
               

                return item;
            }
            catch (Exception ex)
            {
                // Log or print the exception details
                Console.WriteLine($"Exception while adding user: {ex.Message}");

                // Access inner exception if available
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // You might want to throw the exception again or handle it appropriately based on your application logic.
                throw; // Re-throwing the exception after logging.
            }
        }


        public User Update(int id, User updates)
        {
            var item = _context.Users.Find(id);
            if (item != null)
            {
                item.Username = updates.Username;
                
                item.Email = updates.Email;
                _context.SaveChanges();
            }
            return item;
        }
        public User Delete(int id)
        {
            var item = _context.Users.Find(id);
            if (item != null)
            {
                _context.Users.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
    }
}

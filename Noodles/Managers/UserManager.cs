using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Noodles.Models;


namespace Noodles.Managers
{
    public class UserManager
    {
        //    private readonly UserManager<User> _userManager;
        //    private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NoodlesDBContext _context;
        //public UserManager(NoodlesDBContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

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
            return _context.Users.Find(id);
        }
        public User Add(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();

            // Assign the "Customer" role to the newly created user
           // _userManager.AddToRoleAsync(item, "Customer").Wait();

            return item;
        }
        //public async Task AssignRolesToUsers()
        //{
        //    // Find the users by their usernames
        //    var sumi = await _userManager.FindByNameAsync("sumi");
        //    var sabin = await _userManager.FindByNameAsync("sabin");
        //    var saugat = await _userManager.FindByNameAsync("saugat");

        //    // Assign roles to users
        //    if (sumi != null)
        //    {
        //        await _userManager.AddToRoleAsync(sumi, "Admin");
        //    }

        //    if (sabin != null)
        //    {
        //        await _userManager.AddToRoleAsync(sabin, "Customer");
        //    }

        //    if (saugat != null)
        //    {
        //        await _userManager.AddToRoleAsync(saugat, "Customer");
        //    }
        //}

        //public async Task AssignRoleToUser(string username, string roleName)
        //{

        //    var user = await _userManager.FindByNameAsync(username);
        //    if (user != null)
        //    {
        //        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        //        if (!roleExists)
        //        {
        //            // Role doesn't exist, create it
        //            var newRole = new IdentityRole(roleName);
        //            await _roleManager.CreateAsync(newRole);
        //        }

        //        // Assign the role to the user
        //        await _userManager.AddToRoleAsync(user, roleName);
        //    }
        //}


        public User Update(int id, User updates)
        {
            var item = _context.Users.Find(id);
            if (item != null)
            {
                item.Username = updates.Username;
                item.Password = updates.Password;
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

        public async Task<User> FindByNameAsync(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return user != null && user.Password == HashPassword(password);

        }
        private string HashPassword(string password)
        {
            // Replace this with a secure password hashing mechanism
            // This is just a placeholder for demonstration purposes
            return password; // In practice, you should use a secure hashing library
        }
        // In your UserManager after creating users


    }
}

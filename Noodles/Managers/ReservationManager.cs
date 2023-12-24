using Noodles.Models;

namespace Noodles.Managers
{
    public class ReservationManager
    {
        private readonly NoodlesDBContext _context;
        public ReservationManager(NoodlesDBContext context)
        {
            _context = context;
        }

        public List<Reservation> GetAll()
        {
            return _context.Reservations.ToList();
        }
        public Reservation GetById(int id)
        {
            return _context.Reservations.Find(id);
        }
        public Reservation Add(Reservation item, int userId)
        {
            item.UserId = userId;

            _context.Reservations.Add(item);
            _context.SaveChanges();
            return item;
        }
        public Reservation Update(int id, Reservation updates)
        {
            var item = _context.Reservations.Find(id);
            if (item != null)
            {
                item.UserId = updates.UserId;
                item.ReservationDateTime = updates.ReservationDateTime;
                item.TableNumber = updates.TableNumber;
                _context.SaveChanges();
            }
            return item;
        }
        public Reservation Delete(int id)
        {
            var item = _context.Reservations.Find(id);
            if (item != null)
            {
                _context.Reservations.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
    }
}



namespace Noodles.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        // Add other relevant fields

        public void ValidateSubscriptionId()
        {
            if (SubscriptionId < 1)
            {
                throw new ArgumentOutOfRangeException("SubscriptionId must be a positive number!");
            }
        }

        public void ValidateName()
        {
            if (Name == null)
            {
                throw new ArgumentNullException("Name cannot be null. You need to provide a name.");
            }
            else if (Name.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Name needs to be at least 2 characters.");
            }
        }

        public void ValidatePrice()
        {
            if (Price <= 0)
            {
                throw new ArgumentOutOfRangeException("Price must be greater than zero.");
            }
        }

        public void ValidateDiscountPercentage()
        {
            if (DiscountPercentage < 0 || DiscountPercentage > 100)
            {
                throw new ArgumentOutOfRangeException("DiscountPercentage should be between 0 and 100.");
            }
        }

        public override string ToString()
        {
            return $"SubscriptionId {SubscriptionId}, Name {Name}, Price {Price}, DiscountPercentage {DiscountPercentage}";
        }
    }

}

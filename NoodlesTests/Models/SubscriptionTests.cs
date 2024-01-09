using Microsoft.VisualStudio.TestTools.UnitTesting;
using Noodles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodles.Models.Tests
{
    [TestClass()]
    public class SubscriptionTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            Subscription subscription = new Subscription()
            {
                SubscriptionId = 1,
                Name = "Premium",
                Price = 19.99m,
                DiscountPercentage = 10.0m
                // Add other relevant fields
            };

            // Act
            string result = subscription.ToString();

            // Assert
            Assert.AreEqual("SubscriptionId 1, Name Premium, Price 19.99, DiscountPercentage 10.0", result);
        }

        [TestMethod()]
        public void ValidateSubscriptionIdTest()
        {
            // Arrange
            Subscription subscription = new Subscription()
            {
                SubscriptionId = 0,
                Name = "Basic",
                Price = 9.99m,
                DiscountPercentage = 5.0m
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => subscription.ValidateSubscriptionId());
        }

        [TestMethod()]
        public void ValidateNameTest()
        {
            // Arrange
            Subscription subscription = new Subscription()
            {
                SubscriptionId = 2,
                Name = "B",
                Price = 9.99m,
                DiscountPercentage = 5.0m
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => subscription.ValidateName());
        }

        [TestMethod()]
        public void ValidatePriceTest()
        {
            // Arrange
            Subscription subscription = new Subscription()
            {
                SubscriptionId = 3,
                Name = "Standard",
                Price = 0, // Invalid price
                DiscountPercentage = 5.0m
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => subscription.ValidatePrice());
        }

        [TestMethod()]
        public void ValidateDiscountPercentageTest()
        {
            // Arrange
            Subscription subscription = new Subscription()
            {
                SubscriptionId = 4,
                Name = "Premium",
                Price = 29.99m,
                DiscountPercentage = -5.0m // Invalid discount percentage
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => subscription.ValidateDiscountPercentage());
        }
    }
}
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
    public class UserTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            User user = new User()
            {
                UserId = 1,
                Username = "john_doe",
                Email = "john@example.com",
                Role = "Customer",
                // Add other relevant fields
            };

            // Act
            string result = user.ToString();

            // Assert
            Assert.AreEqual("UserId 1, Username john_doe, Email john@example.com, Role Customer", result);
        }

        [TestMethod()]
        public void ValidateUserIdTest()
        {
            // Arrange
            User user = new User()
            {
                UserId = 0,
                Username = "jane_doe",
                Email = "jane@example.com",
                Role = "Customer",
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => user.ValidateUserId());
        }

        [TestMethod()]
        public void ValidateUsernameTest()
        {
            // Arrange
            User user = new User()
            {
                UserId = 2,
                Username = "u",
                Email = "user@example.com",
                Role = "Customer",
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => user.ValidateUsername());
        }

        [TestMethod()]
        public void ValidateEmailTest()
        {
            // Arrange
            User user = new User()
            {
                UserId = 3,
                Username = "test_user",
                Email = "invalidemail", // Invalid email
                Role = "Customer",
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentException>(() => user.ValidateEmail());
        }

        [TestMethod()]
        public void ValidateRoleTest()
        {
            // Arrange
            User user = new User()
            {
                UserId = 4,
                Username = "admin_user",
                Email = "admin@example.com",
                Role = "InvalidRole", // Invalid role
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentException>(() => user.ValidateRole());
        }
    }
}
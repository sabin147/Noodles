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
    public class ReservationTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            Reservation reservation = new Reservation()
            {
                ReservationId = 1,
                UserId = 101,
                ReservationDateTime = DateTime.Now,
                TableNumber = 5
                // Add other relevant fields
            };

            // Act
            string result = reservation.ToString();

            // Assert
            Assert.AreEqual("ReservationId 1, UserId 101, ReservationDateTime " + reservation.ReservationDateTime.ToString() + ", TableNumber 5", result);
        }

        [TestMethod()]
        public void ValidateReservationIdTest()
        {
            // Arrange
            Reservation reservation = new Reservation()
            {
                ReservationId = 0,
                UserId = 101,
                ReservationDateTime = DateTime.Now,
                TableNumber = 5
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reservation.ValidateReservationId());
        }

        [TestMethod()]
        public void ValidateUserIdTest()
        {
            // Arrange
            Reservation reservation = new Reservation()
            {
                ReservationId = 1,
                UserId = 0,
                ReservationDateTime = DateTime.Now,
                TableNumber = 5
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reservation.ValidateUserId());
        }

        [TestMethod()]
        public void ValidateReservationDateTimeTest()
        {
            // Arrange
            Reservation reservation = new Reservation()
            {
                ReservationId = 1,
                UserId = 101,
                ReservationDateTime = DateTime.Now.AddDays(-1), // Invalid past date
                TableNumber = 5
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reservation.ValidateReservationDateTime());
        }

        [TestMethod()]
        public void ValidateTableNumberTest()
        {
            // Arrange
            Reservation reservation = new Reservation()
            {
                ReservationId = 1,
                UserId = 101,
                ReservationDateTime = DateTime.Now,
                TableNumber = 0 // Invalid table number
                // Add other relevant fields
            };

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reservation.ValidateTableNumber());
        }
    }
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Noodles.Models;

public partial class User 
{
    [Key]
    [Column("UserID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(256)]
    public byte[] PasswordHash { get; set; }

    [Required]
    [StringLength(256)]
    public byte[] PasswordSalt { get; set; }

    
    [StringLength(100)]
    public string Email { get; set; }
    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey("SubscriptionId")]
    public int? SubscriptionId { get; set; }

    [JsonIgnore]
    public virtual Subscription Subscription { get; set; }

    public string Role { get; set; }

    public void ValidateUserId()
    {
        if (UserId < 1)
        {
            throw new ArgumentOutOfRangeException("UserId must be a positive number!");
        }
    }

    public void ValidateUsername()
    {
        if (Username == null)
        {
            throw new ArgumentNullException("Username cannot be null. You need to provide a username.");
        }
        else if (Username.Length < 2)
        {
            throw new ArgumentOutOfRangeException("Username needs to be at least 2 characters.");
        }
    }

    public void ValidateEmail()
    {
        // You may use a regular expression or other validation logic for email validation.
        // For simplicity, I'm checking if it contains '@'.
        if (Email == null || !Email.Contains('@'))
        {
            throw new ArgumentException("Invalid email format.");
        }
    }

    public void ValidateRole()
    {
        // You may have specific roles that are allowed. For simplicity, I'm checking if it's not empty.
        if (string.IsNullOrWhiteSpace(Role))
        {
            throw new ArgumentException("Role cannot be null or empty.");
        }
    }

}
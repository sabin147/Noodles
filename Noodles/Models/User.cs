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
}
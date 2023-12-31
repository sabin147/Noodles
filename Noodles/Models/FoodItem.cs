﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Noodles.Models;

public partial class FoodItem
{
    [Key]
    [Column("FoodItemID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FoodItemId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [JsonIgnore]
    [InverseProperty("FoodItem")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
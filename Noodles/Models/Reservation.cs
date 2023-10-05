﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Noodles.Models;

public partial class Reservation
{
    [Key]
    [Column("ReservationID")]
    public int ReservationId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReservationDateTime { get; set; }

    public int? TableNumber { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Reservations")]
    public virtual User User { get; set; }
}
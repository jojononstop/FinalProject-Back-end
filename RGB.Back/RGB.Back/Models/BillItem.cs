﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RGB.Back.Models;

public partial class BillItem
{
    public int Id { get; set; }

    public int BillDetailId { get; set; }

    public int GameId { get; set; }

    public decimal Quantity { get; set; }

    public decimal Total { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual BillDetail BillDetail { get; set; }

    public virtual Game Game { get; set; }
}
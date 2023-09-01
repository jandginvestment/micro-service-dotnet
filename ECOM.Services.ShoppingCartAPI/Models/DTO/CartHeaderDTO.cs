﻿namespace ECOM.Services.ShoppingCartAPI.Models.DTO;

public class CartHeaderDTO
{
    public required int CartHeaderID { get; set; }
    public required string UserID { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; } = 0;
    public double CartTotal { get; set; } = 0;
}
﻿using System.ComponentModel.DataAnnotations;

namespace ECOM.Services.OrderAPI.Models.DTO;


public class ProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductId { get; set; }
    public string CategoryName { get; set; }

    public string? ImageUrl { get; set; }
    public double Price { get; set; }
    public string? ImageLocalPath { get; set; }

    [Range(1, 100)]
    public int Count { get; set; } = 1;
    public IFormFile? Image { get; set; }
}
﻿using Microsoft.AspNetCore.Mvc;

namespace ECOM.Web.Models;

[Bind]
public class ShoppingCartDTO
{
    public CartHeaderDTO CartHeader { get; set; }
    public IEnumerable<CartDetailDTO>? CartDetails { get; set; } = Enumerable.Empty<CartDetailDTO>();

}

using ECOM.Services.OrderAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOM.Services.OrderAPI.Models;

public class OrderDetail
{
    [Key]
    public int OrderDetailID { get; set; }
    public int OrderHeaderID { get; set; }

    [ForeignKey(nameof(OrderHeaderID))]
    public OrderHeader? OrderHeader { get; set; }

    public int ProductID { get; set; }
    [NotMapped]
    public ProductDTO? Product { get; set; }
    public int Count { get; set; }

    public string ProductName { get; set; }
    public double Price { get; set; }
}

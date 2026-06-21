using System.ComponentModel.DataAnnotations;
using OrderApi.Models;

namespace OrderApi.Dto;

public class UpdateOrderStatusRequest
{
    [Required]
    public OrderStatus Status { get; set; }
}

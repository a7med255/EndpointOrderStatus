using OrderApi.Models;

namespace OrderApi.Dto;

public class OrderStatusResponse
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}

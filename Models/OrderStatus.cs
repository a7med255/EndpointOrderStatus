namespace OrderApi.Models;

public enum OrderStatus
{
    Created,
    Confirmed,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

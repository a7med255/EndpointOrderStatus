using OrderApi.Models;

namespace OrderApi.Services;

public static class OrderStatusTransitionValidator
{
    private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> AllowedTransitions = new()
    {
        [OrderStatus.Created] = [OrderStatus.Confirmed, OrderStatus.Cancel],
        [OrderStatus.Confirmed] = [OrderStatus.Processing, OrderStatus.Cancel],
        [OrderStatus.Processing] = [OrderStatus.Shipped, OrderStatus.Cancel],
        [OrderStatus.Shipped] = [OrderStatus.Delivered],
        [OrderStatus.Delivered] = [],
        [OrderStatus.Cancel] = []
    };

    public static bool IsValidTransition(OrderStatus currentStatus, OrderStatus newStatus) =>
        AllowedTransitions.TryGetValue(currentStatus, out var allowed) && allowed.Contains(newStatus);
}

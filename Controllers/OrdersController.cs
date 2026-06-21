using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Models;
using OrderApi.Services;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPut("{orderId}/status")]
    [ProducesResponseType(typeof(OrderStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderStatusResponse>> UpdateStatus(
        int orderId,
        [FromBody] UpdateOrderStatusRequest request)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
        {
            return NotFound();
        }

        var oldStatus = order.Status;
        var newStatus = request.Status;

        if (!OrderStatusTransitionValidator.IsValidTransition(oldStatus, newStatus))
        {
            _logger.LogWarning(
                "Invalid order status transition attempted. OrderId: {OrderId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}",
                orderId,
                oldStatus,
                newStatus);

            return BadRequest(new
            {
                error = $"Cannot transition from '{oldStatus}' to '{newStatus}'."
            });
        }

        order.Status = newStatus;
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Order status updated successfully. OrderId: {OrderId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}",
            orderId,
            oldStatus,
            newStatus);

        return Ok(new OrderStatusResponse
        {
            OrderId = order.Id,
            Status = order.Status
        });
    }
}

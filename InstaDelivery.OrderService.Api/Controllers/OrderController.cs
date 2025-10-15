using InstaDelivery.OrderService.Application.Dto;
using InstaDelivery.OrderService.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.OrderService.Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken token = default)
    {
        try
        {
            var order = await orderService.CreateOrderAsync(request, token);
            return CreatedAtRoute(nameof(GetOrderByIdAsync), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:guid}", Name = "GetOrderByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderByIdAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            var order = await orderService.GetOrderByIdAsync(id, token);
            if (order is null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("customer/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByCustomerIdAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            var orders = await orderService.GetOrdersByCustomerIdAsync(id, token);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

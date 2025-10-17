using InstaDelivery.OrderService.Api.Constants;
using InstaDelivery.OrderService.Application.Dto;
using InstaDelivery.OrderService.Application.Services.Contracts;
using InstaDelivery.OrderService.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.OrderService.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize(Policy = AuthPolicy.BasicAccess)]
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
            return Ok(order);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = AuthPolicy.ElevatedAccess)]
    public async Task<IActionResult> DeleteOrderAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            await orderService.DeleteOrderAsync(id, token);
            return NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpGet("availableOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableOrdersAsync(CancellationToken token = default)
    {
        try
        {
            var userName = HttpContext.User.Identity?.Name;
            var orders = await orderService.GetAvailableOrdersAsync(token);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
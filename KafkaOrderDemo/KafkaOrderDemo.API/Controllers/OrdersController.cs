using KafkaOrderDemo.Application.Commands.CreateOrder;
using KafkaOrderDemo.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KafkaOrderDemo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(CreateOrderHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var cmd = new CreateOrderCommand(dto);
        await handler.Handle(cmd);

        return Ok(new 
        { 
            orderId = Guid.NewGuid(),
            message = "Order sent to Kafka successfully."
        });
    }
}
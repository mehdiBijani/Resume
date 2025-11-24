using KafkaOrderDemo.Application.DTOs;

namespace KafkaOrderDemo.Application.Commands.CreateOrder;

public class CreateOrderCommand(CreateOrderDto dto)
{
    public CreateOrderDto Dto { get; } = dto;
}
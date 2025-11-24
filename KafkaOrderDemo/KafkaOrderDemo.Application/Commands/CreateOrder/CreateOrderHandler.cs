using System.Text.Json;
using KafkaOrderDemo.Domain.Entities;
using KafkaOrderDemo.Domain.Interfaces;

namespace KafkaOrderDemo.Application.Commands.CreateOrder;

public class CreateOrderHandler(IKafkaProducer producer)
{
    public async Task Handle(CreateOrderCommand command)
    {
        var order = new Order(command.Dto.Customer, command.Dto.Amount);

        string message = JsonSerializer.Serialize(order);

        await producer.SendAsync("orders-topic", message);
    }
}
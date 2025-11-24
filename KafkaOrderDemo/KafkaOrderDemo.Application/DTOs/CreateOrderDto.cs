namespace KafkaOrderDemo.Application.DTOs;

public class CreateOrderDto
{
    public string Customer { get; set; }
    public decimal Amount { get; set; }
}
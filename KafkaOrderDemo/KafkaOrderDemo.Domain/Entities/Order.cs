namespace KafkaOrderDemo.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Customer { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Order(string customer, decimal amount)
    {
        Customer = customer;
        Amount = amount;
    }
}
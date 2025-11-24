using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using KafkaOrderDemo.API.Hubs;
using KafkaOrderDemo.Domain.Entities;
using KafkaOrderDemo.Domain.Interfaces;
using KafkaOrderDemo.Infrastructure.Persistence;

namespace KafkaOrderDemo.API.BackgroundServices;

public class KafkaOrderConsumerService : BackgroundService
{
    private readonly IKafkaConsumer _consumer;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaOrderConsumerService(IKafkaConsumer consumer, IServiceScopeFactory scopeFactory)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _consumer.Consume("orders-topic", async (msg) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var order = JsonSerializer.Deserialize<Order>(msg);
                db.Add(order);
                await db.SaveChangesAsync();
                var hub = scope.ServiceProvider.GetRequiredService<IHubContext<OrderHub>>();
                await hub.Clients.All.SendAsync("NewOrder", order);
                Console.WriteLine($"[Saved to DB] {order.Customer}, {order.Amount}");
            }, stoppingToken);
        });
    }
}
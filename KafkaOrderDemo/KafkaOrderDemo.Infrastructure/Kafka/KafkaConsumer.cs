using Confluent.Kafka;
using KafkaOrderDemo.Domain.Interfaces;

namespace KafkaOrderDemo.Infrastructure.Kafka;

public class KafkaConsumer(string bootstrapServers, string groupId) : IKafkaConsumer
{
    private readonly ConsumerConfig _config = new()
    {
        BootstrapServers = bootstrapServers,
        GroupId = groupId,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    public void Consume(string topic, Func<string, Task> onMessage, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Null, string>(_config).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume(cancellationToken);

                _ = onMessage(result.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
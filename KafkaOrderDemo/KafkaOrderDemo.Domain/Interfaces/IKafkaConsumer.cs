namespace KafkaOrderDemo.Domain.Interfaces;

public interface IKafkaConsumer
{
    void Consume(string topic, Func<string, Task> onMessage, CancellationToken cancellationToken);
}
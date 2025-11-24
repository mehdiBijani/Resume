using KafkaOrderDemo.Domain.Interfaces;
using KafkaOrderDemo.Infrastructure.Kafka;

namespace KafkaOrderDemo.API.Configs;

public static class KafkaConfig
{
    public static void AddKafka(this IServiceCollection services, IConfiguration config)
    {
        string servers = config["Kafka:BootstrapServers"];
        string groupId = "order-consumer-group";

        services.AddSingleton<IKafkaProducer>(new KafkaProducer(servers));
        services.AddSingleton<IKafkaConsumer>(new KafkaConsumer(servers, groupId));
    }
}
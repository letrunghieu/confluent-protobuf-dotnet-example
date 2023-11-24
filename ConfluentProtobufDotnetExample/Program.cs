using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using ConfluentProtobufDotnetExample;
using Google.Protobuf;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("./appsettings.json")
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();
    
var producerConfig = configuration.GetSection("Producer").Get<ProducerConfig>().ThrowIfContainsNonUserConfigurable();
var schemaRegistryConfig = configuration.GetSection("SchemaRegistry").Get<SchemaRegistryConfig>();
var topicName = configuration.GetValue<string>("General:TopicName");
var sampleSize = configuration.GetValue<int>("General:SampleSize");

using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
using var producer = new ProducerBuilder<Null, CustomerEvent>(producerConfig)
    .SetValueSerializer(new ProtobufSerializer<CustomerEvent>(schemaRegistry))
    .Build();

var formatter = new JsonFormatter(JsonFormatter.Settings.Default.WithIndentation());
for (var i = 0; i < sampleSize; i++)
{
    var messsage = new CustomerEvent()
    {
        CustomerId = i,
        EventName = i.ToString(),
        Data = i.ToString() + " data",
        CreatedDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
    };
    Console.WriteLine(formatter.Format(messsage));
    await producer.ProduceAsync(topicName, new Message<Null, CustomerEvent>() { Value = messsage });
}
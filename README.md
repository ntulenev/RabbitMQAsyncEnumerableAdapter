# RabbitAdapter
### Adapter that helps use RabbitMq consumer as AsyncEnumerator

```C#
var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);

// Create rabbit adapter
RabbitAdapter ra = new RabbitAdapter(10, messageId => channel.BasicAck(messageId, false));

// Connect adapter with consumer event handler
consumer.Received += ra.ConsumeData;

channel.BasicConsume(queue: "hello",
                     autoAck: false,
                     consumer: consumer);

// Use adapter as async enumerator
await foreach (var data in ra.WithCancellation(CancellationToken.None))
{
    var body = data.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(body);
}

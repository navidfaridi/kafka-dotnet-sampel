using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace KafkaConsumerConsole;

public class KafkaWorker : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;

    public KafkaWorker()
    {
        var config = new ConsumerConfig
        {
            GroupId = "product-consumers",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("product_requests");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            var request = JsonSerializer.Deserialize<PagedRequest>(consumeResult.Message.Value);

            //using var dbConnection = new SqlConnection("your_connection_string");
            //var query = "SELECT * FROM Products ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            List<Product> products =
            [
                new Product { Id = 1, Name = "p1", Price = 1242453 },
                new Product { Id = 2, Name = "p2", Price = 32422 },
            ];
            //await dbConnection.QueryAsync<Product>(query, new { Offset = (request.Page - 1) * request.PageSize, request.PageSize });

            // اینجا می‌توانید نتایج را به یک سرویس دیگر ارسال کنید یا به هر روشی که نیاز دارید از آن‌ها استفاده کنید
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}

public class PagedRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
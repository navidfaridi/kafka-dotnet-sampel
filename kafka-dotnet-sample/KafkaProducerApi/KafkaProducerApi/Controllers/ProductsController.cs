using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KafkaProducerApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProducer<Null, string> _producer;

    public ProductsController()
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedProducts(int page = 1, int pageSize = 10)
    {
        var request = new { Page = page, PageSize = pageSize };
        var message = JsonSerializer.Serialize(request);

        var topic = "product_requests";
        var deliveryReport = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

        return Accepted($"Message sent to Kafka, Offset: {deliveryReport.Offset}");
    }
}
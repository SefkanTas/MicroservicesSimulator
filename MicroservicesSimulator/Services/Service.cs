using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class Service
{
    public string Name { get; set; }
    public State State { get; set; }
    private TracerProvider _tracerProvider;

    private ILogger _logger;

    public Service(string name, State state = State.Normal)
    {
        Name = name;
        State = state;
        // _tracerProvider = BuildTraceProvider();

        // var loggerFactory = LoggerFactory.Create(build => 
        //     build.AddOpenTelemetry()
        // );
        // _logger = loggerFactory.CreateLogger<Service>();
        // _logger.LogInformation("yes tout va bien");
    }

    public void Call()
    {
        var myActivitySource = new ActivitySource(Name);
        Thread.Sleep(GetRandomLantency());
        using var activity = myActivitySource.StartActivity($"Span from {Name}");
        // _logger.LogInformation("yes info");
        // _logger.LogTrace("yes trace");
        
        // activity?.Stop();
    }

    private int GetRandomLantency()
    {
        Random random = new Random();
        if (State == State.Normal)
        {
            return random.Next(1, 20);
        }
        return random.Next(100, 200);
    }
    
    private TracerProvider BuildTraceProvider()
    {
        return Sdk.CreateTracerProviderBuilder()
            .AddSource(Name)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: Name))
            .AddZipkinExporter()
            .AddJaegerExporter()
            .AddConsoleExporter()
            .Build();
    }
}
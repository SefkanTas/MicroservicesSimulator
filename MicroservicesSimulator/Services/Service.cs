using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using System.Collections;
using MicroservicesSimulator.Services.States;

namespace MicroservicesSimulator.Services;

public class Service
{
    public string Name { get; set; }
    internal IServiceState State;

    // private ILogger _logger;

    public Service(string name, IServiceState state)
    {
        Name = name;
        State = state;
        // State = state;
        // _tracerProvider = BuildTraceProvider();

        // var loggerFactory = LoggerFactory.Create(build => 
        //     build.AddOpenTelemetry()
        // );
        // _logger = loggerFactory.CreateLogger<Service>();
        // _logger.LogInformation("yes tout va bien");
    }

    public Service(string name)
    {
        Name = name;
        State = new NormalState();
        State.UpdateState(this);
    }

    public void Call(Queue<Service>? services)
    {
        // var myActivitySource = new ActivitySource(Name);
        // Thread.Sleep(GetRandomLantency());
        // using var activity = myActivitySource.StartActivity($"Span from {Name}");

        if (services == null)
        {
            return;
        }
        if (services.Count == 0)
        {
            State.Call(this, null);
        }
        else
        {
            State.Call(this, services);
        }
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
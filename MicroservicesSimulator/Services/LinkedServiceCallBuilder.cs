using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class LinkedServiceCallBuilder
{

    private List<Service> _services;

    public LinkedServiceCallBuilder()
    {
        WithNumberOfServices(3);
    }

    public LinkedServiceCallBuilder WithNumberOfServices(int nb)
    {
        if (nb < 2)
        {
            return this;
        }

        _services = new List<Service>();
        for (var i = 0; i < nb; i++)
        {
            _services.Add(new Service($"service-{i}"));
        }

        return this;
    }

    private TracerProvider BuildTraceProvider()
    {
        var traceProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: "Microservices-Simulator")
            )
            .AddZipkinExporter()
            .AddJaegerExporter()
            .AddConsoleExporter();

        foreach (var service in _services)
        {
            traceProviderBuilder.AddSource(service.Name);
        }

        return traceProviderBuilder.Build();
    }
    
    public LinkedServiceCall Build()
    {
        return new LinkedServiceCall(_services, BuildTraceProvider());
    }
}
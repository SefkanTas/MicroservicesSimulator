using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class ServiceManagerBuilder
{

    private List<Service> _services;

    public ServiceManagerBuilder()
    {
        _services = new List<Service>();
        WithNumberOfServices(3);
    }

    public ServiceManagerBuilder WithNumberOfServices(int nb)
    {
        // if (nb < 2)
        // {
        //     return this;
        // }

        _services = new List<Service>();
        for (var i = 0; i < nb; i++)
        {
            _services.Add(new Service($"service-{i+1}"));
        }

        return this;
    }

    private TracerProvider BuildTraceProvider()
    {
        var traceProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: "Microservices-Simulator-1")
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
    
    public ServiceManager Build()
    {
        return new ServiceManager(_services, BuildTraceProvider());
    }
}
using MicroservicesSimulator.Services.States;
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
    }

    private Service CreateService(string name, IServiceState state) => new Service(name, state);
    
    private ServiceManagerBuilder WithServices(int nb, IServiceState state)
    {
        var servicesCount = _services.Count;
        
        for (var i = 0; i < nb; i++)
        {
            var service = CreateService($"service-{servicesCount + i + 1}", state);
            _services.Add(service);
        }

        return this;
    }

    public ServiceManagerBuilder WithNormalServices(int nb)
    {
        return WithServices(nb, new NormalState());
    }

    public ServiceManagerBuilder WithSlowServices(int nb)
    {
        return WithServices(nb, new SlowState());
    }
    
    public ServiceManagerBuilder WithDownServices(int nb)
    {
        return WithServices(nb, new DownState());
    }

    private TracerProvider BuildTraceProvider()
    {
        var traceProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: "Microservices-Simulator-test")
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
using MicroservicesSimulator.Services.States;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class ServiceManagerBuilder
{
    private List<Service> _services;
    private string _serviceName;
    
    public ServiceManagerBuilder()
    {
        _services = new List<Service>();
        _serviceName = "Microservices-Simulator";
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

    public ServiceManagerBuilder WithNormalServices(int nb, bool isStable = false) 
    {
        return WithServices(nb, new NormalState(isStable));
    }

    public ServiceManagerBuilder WithSlowServices(int nb, bool isStable = false)
    {
        return WithServices(nb, new SlowState(isStable));
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
                    .AddService(serviceName: _serviceName)
            )
            .AddZipkinExporter()
            .AddJaegerExporter();
            // .AddConsoleExporter();

        foreach (var service in _services)
        {
            traceProviderBuilder.AddSource(service.Name);
        }
        
        return traceProviderBuilder.Build();
    }

    public ServiceManagerBuilder WithServiceName(string name)
    {
        _serviceName = name;
        return this;
    }
    
    public ServiceManager Build()
    {
        return new ServiceManager(_services, BuildTraceProvider());
    }
}
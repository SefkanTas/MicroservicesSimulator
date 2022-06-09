using System.Diagnostics;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class ServiceManager
{
    private List<Service> _services;
    private TracerProvider _traceProvider;

    public ServiceManager(List<Service> services, TracerProvider tracerProvider)
    {
        _services = services;
        _traceProvider = tracerProvider;
    }
    
    public void Run(int iteration = 10)
    {
        for (var i = 0; i < iteration; i++)
        {
            var servicesQueue = new Queue<Service>(_services);
            var firstService = servicesQueue.Dequeue();
            firstService.Call(servicesQueue);
        }
    }
    
}
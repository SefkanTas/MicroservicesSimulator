using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services;

public class LinkedServiceCall
{
    private List<Service> _services;
    private TracerProvider _traceProvider;

    public LinkedServiceCall(List<Service> services, TracerProvider tracerProvider)
    {
        _services = services;
        _traceProvider = tracerProvider;
    }

    public void Run(int iteration = 10)
    {
        for (var i = 0; i < iteration; i++)
        {
            CallServices();
        }
        var myActivitySource = new ActivitySource("test");
        using var activity = myActivitySource.StartActivity($"Span from test");
        // activity?.Stop();
    }

    private void CallServices()
    {
        foreach (var service in _services)
        {
            UpdateServiceState(service);
            if (service.State == State.Down)
            {
                return;
            }
            GenerateLatency(service);
            service.Call();
        }
    }

    private void GenerateLatency(Service service)
    {
        Thread.Sleep(GetRandomLatency(service));
    }
    
    private int GetRandomLatency(Service service)
    {
        var random = new Random();
        if (service.State == State.Normal)
        {
            return random.Next(1, 20);
        }
        return random.Next(100, 200);
    }

    private void UpdateServiceState(Service service)
    {
        var random = new Random();
        var number = random.Next(0, 100);
        switch (number)
        {
            case 0:
                service.State = State.Down;
                break;
            case >= 1 and <= 10:
                service.State = State.Slow;
                break;
            default:
                service.State = State.Normal;
                break;
        }
    }
}
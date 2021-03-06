using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator.Services.States;

public abstract class UpState : IServiceState
{

    internal bool IsStable;

    public UpState(bool isStable = false)
    {
        IsStable = isStable;
    }

    public void Call(Service service, Queue<Service>? nextServices)
    {
        var myActivitySource = new ActivitySource(service.Name);
        using var activity = myActivitySource.StartActivity($"Span from {service.Name}");
        GenerateLatency();
        UpdateState(service);
        var nextService = nextServices?.Dequeue();
        nextService?.Call(nextServices);
    }

    public abstract void UpdateState(Service service);
    protected abstract void GenerateLatency();
}
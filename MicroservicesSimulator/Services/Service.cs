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

    public Service(string name, IServiceState state)
    {
        Name = name;
        State = state;
    }

    public Service(string name)
    {
        Name = name;
        State = new NormalState();
        State.UpdateState(this);
    }

    public void Call(Queue<Service>? services)
    {
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
}
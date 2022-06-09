namespace MicroservicesSimulator.Services.States;

public class DownState : IServiceState
{
    public void Call(Service service, Queue<Service>? nextServices)
    {
        //Does nothing to simulate a down service
    }

    public void UpdateState(Service service)
    {
        //Does nothing to simulate a down service
    }
}
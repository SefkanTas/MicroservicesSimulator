namespace MicroservicesSimulator.Services.States;

public interface IServiceState
{
    void Call(Service service, Queue<Service>? nextServices);
    void UpdateState(Service service);
}
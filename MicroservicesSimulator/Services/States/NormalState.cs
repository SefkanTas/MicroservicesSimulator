
namespace MicroservicesSimulator.Services.States;

public class NormalState : UpState
{

    public NormalState(bool isStable = false):base(isStable)
    { }
    
    public override void UpdateState(Service service)
    {
        if (IsStable)
        {
            return;
        }
        var random = new Random();
        var number = random.Next(1, 100);
        switch (number)
        {
            case 1:
                service.State = new DownState();
                break;
            case >= 1 and <= 10:
                service.State = new SlowState();
                break;
            default:
                service.State = new NormalState();
                break;
        }
    }

    protected override void GenerateLatency()
    {
        var random = new Random();
        var duration = random.Next(1, 50);

        Thread.Sleep(duration);
    }
}
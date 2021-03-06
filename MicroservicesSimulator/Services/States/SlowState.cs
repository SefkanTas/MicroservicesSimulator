
namespace MicroservicesSimulator.Services.States;

public class SlowState : UpState
{
    
    public SlowState(bool isStable = false):base(isStable)
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
            case >= 1 and <= 10:
                service.State = new DownState();
                break;
            case >= 11 and <= 60:
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
        var duration = random.Next(300, 500);

        Thread.Sleep(duration);
    }
}
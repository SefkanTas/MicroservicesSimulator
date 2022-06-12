using MicroservicesSimulator.Services;

namespace MicroservicesSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceManagerBuilder = new ServiceManagerBuilder();
            var serviceManager = serviceManagerBuilder
                .WithNormalServices(6)
                .Build();
            serviceManager.Run(50);
        }
    }
}
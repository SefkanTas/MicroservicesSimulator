using MicroservicesSimulator.Services;

namespace MicroservicesSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var serviceManagerBuilder = new ServiceManagerBuilder();
            var serviceManager = serviceManagerBuilder
                // .WithDownServices(1)
                .WithNormalServices(10)
                .Build();
            serviceManager.Run(1);
        }
    }
}
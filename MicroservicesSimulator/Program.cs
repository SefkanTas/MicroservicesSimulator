// See https://aka.ms/new-console-template for more information

using MicroservicesSimulator.Services;

Console.WriteLine("Hello, World!");

var serviceManagerBuilder = new ServiceManagerBuilder();
var serviceManager = serviceManagerBuilder.WithNumberOfServices(10).Build();
serviceManager.Run(10);

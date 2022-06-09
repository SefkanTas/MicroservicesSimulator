// See https://aka.ms/new-console-template for more information

using MicroservicesSimulator.Services;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

Console.WriteLine("Hello, World!");

// MyClass.Run();


var linkedServiceCallBuilder = new LinkedServiceCallBuilder();
var linkedServiceCall = linkedServiceCallBuilder.Build();
linkedServiceCall.Run(1);

// var service = new Service("Service-A");
//
// var stack = new Stack<Service>();
// stack.Push(new Service("Service-B"));
// stack.Push(new Service("Service-C"));
// stack.Push(service);
// stack.Push(service);
//
// service.Call(stack);
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroservicesSimulator;

public class MyClass
{

    public static void Run()
    {
        test1();

    }

    private static async void test3()
    { 
        var myActivitySource = new ActivitySource("OpenTelemetry.Demo.Jaeger");

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: "DemoApp",
                serviceVersion: "1.0.0"))
            .AddSource("OpenTelemetry.Demo.Jaeger")
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddJaegerExporter()
            .Build();

        using var parent = myActivitySource.StartActivity("JaegerDemo");
        parent?.SetTag("test", "yess");

        using var client = new HttpClient();
        using (var slow = myActivitySource.StartActivity("SomethingSlow"))
        {
            await client.GetStringAsync("https://httpstat.us/200?sleep=1000");
            await client.GetStringAsync("https://httpstat.us/200?sleep=1000");
        }

        using (var fast = myActivitySource.StartActivity("SomethingFast"))
        {
            await client.GetStringAsync("https://httpstat.us/301");
        }
    }
    
    private static void test2()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.AddConsoleExporter();
            });
        });
        
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogInformation("Hello from {name} {price}.", "tomato", 2.99);
    }

    private static void test1()
    {

        var log = new Logger<MyClass>(new LoggerFactory());
        log.LogInformation("log info test");
        var serviceName = "sefkan-test";
        
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName))
            .AddZipkinExporter()
            .AddJaegerExporter()
            // .AddZipkinExporter(o =>
            // {
            //     o.Endpoint = new Uri("http://localhost:9411");
            // })
            .AddConsoleExporter()
            // Other setup code, like setting a resource goes here too
            .Build();

        var MyActivitySource = new ActivitySource(serviceName);

        using var activity = MyActivitySource.StartActivity("SayHello");
        activity?.SetTag("foo", 1);
        activity?.SetTag("bar", "Hello, World!");
        activity?.SetTag("baz", new int[] { 1, 2, 3 });
        
        // Thread.Sleep(2000);
        using var act2 = MyActivitySource.StartActivity("SayHelloChild");
        act2?.SetTag("yes", "sefkan");
        
        // Thread.Sleep(4000);
        // var span = tracerProvider.GetTracer("a", "a").StartSpan("yes");
        //
        // span.AddEvent("Sefkan event name start");
        // span.AddEvent("Sefkan event name end");
        // span.End();
    }
}
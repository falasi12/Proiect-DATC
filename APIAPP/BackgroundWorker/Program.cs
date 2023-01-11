using APIAPP.Models;
using APIAPP;
using APIAPP.Repositories;
using Azure.Messaging.ServiceBus;
using LanguageExt;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the processor that reads and processes messages from the queue
ServiceBusProcessor processor;
//InfoContext dbContext;

var contextOptions = new DbContextOptionsBuilder<InfoContext>()
    .UseSqlServer(@"Server=tcp:ambrosiadatc.database.windows.net,1433;Initial Catalog=AmbrosiaDB;Persist Security Info=False;User ID=cr1ss;Password=ProiectDATC123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
    .Options;

using var dbContext = new InfoContext(contextOptions);
PointOfInterestRepository poiRepo = new PointOfInterestRepository(dbContext);
double distanceBetween;

// The Service Bus client types are safe to cache and use as a singleton for the lifetime
// of the application, which is best practice when messages are being published or read
// regularly.
//
// Set the transport type to AmqpWebSockets so that the ServiceBusClient uses port 443. 
// If you use the default AmqpTcp, make sure that ports 5671 and 5672 are open.

// TODO: Replace the <NAMESPACE-CONNECTION-STRING> and <QUEUE-NAME> placeholders
var clientOptions = new ServiceBusClientOptions()
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};
client = new ServiceBusClient("Endpoint=sb://datcservicebusproject.servicebus.windows.net/;SharedAccessKeyName=Admin;SharedAccessKey=N8uTNibOhllgmetihPnl/KGDO9z4D8DcnH4NIxrsXzk=;EntityPath=datcprojectqueue", clientOptions);

// create a processor that we can use to process the messages
// TODO: Replace the <QUEUE-NAME> placeholder
processor = client.CreateProcessor("datcprojectqueue", new ServiceBusProcessorOptions());

try
{
    // add handler to process messages
    processor.ProcessMessageAsync += MessageHandler;

    // add handler to process any errors
    processor.ProcessErrorAsync += ErrorHandler;

    // start processing 
    while (true)
    {
        await processor.StartProcessingAsync();
        Thread.Sleep(500);
        await processor.StopProcessingAsync();
    }
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

double GetDistance(double lat1, double lon1, double lat2, double lon2)
{
    var R = 6371; // Radius of the earth in km
    var dLat = ToRadians(lat2 - lat1);
    var dLon = ToRadians(lon2 - lon1);
    var a =
        Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    var d = R * c; // Distance in km
    return d*1000;
}

double ToRadians(double deg)
{
    return deg * (Math.PI / 180);
}
// handle received messages
 async  Task MessageHandler(ProcessMessageEventArgs args)
{
    string message = args.Message.Body.ToString();

    PointsOfInterestDTO p = JsonConvert.DeserializeObject<PointsOfInterestDTO>(message);

    List<PointsOfInterestDTO> allPointsDTO = poiRepo.TryGetPointsOfInterest().Result;

    Console.WriteLine($"Received: {message}");
    bool isInRange = false;

    foreach (var entry in allPointsDTO)
    {   double d;
        //entry.Latitude, entry.Longitude);
        d = GetDistance(p.Latitude, p.Longitude, entry.Latitude, entry.Longitude);
       if (d < entry.Radius)
        {
            //Is in range 
            isInRange = true;
            Console.WriteLine("Ambrosia was reported near at: "+ entry.Latitude+ "; "+ entry.Longitude);
            entry.Radius += 50.0; //Extending radius with 10km

            dbContext.Update(entry);
            await dbContext.SaveChangesAsync();

            break;
        }
    }

    if (!isInRange)
    {
        Console.WriteLine("New Point of interest");
        //Saving into database
        dbContext.Add(p);
        await dbContext.SaveChangesAsync();

    }

    await args.CompleteMessageAsync(args.Message);
}

// handle any errors when receiving messages
Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
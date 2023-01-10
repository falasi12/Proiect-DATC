using BackgroundWorker.Models;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using Microsoft.AspNetCore.Mvc.Filters;
using LanguageExt.Pipes;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace BackgroundWorker.Repositories
{
    public class PointOfInterestRepository : IPointOfInterestRepository
    {
        private readonly InfoContext dbContext;
        ServiceBusClient client;
        ServiceBusSender sender;

        public PointOfInterestRepository(InfoContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<PointsOfInterestDTO>> TryGetPointsOfInterest()
        {
            return (await(
                                    from g in dbContext.PointsOfInterest
                                    where g.Archived == false
                                    select new { g.Id, g.Latitude, g.Longitude, g.Radius })
                                    .AsNoTracking()
                                    .ToListAsync())
                                    .Select(result => new PointsOfInterestDTO(
                                                                            id: result.Id,
                                                                            latitude: result.Latitude,
                                                                            longitude: result.Longitude,
                                                                            radius: result.Radius)
                                                )
                                                .ToList();
        }

        public async Task<bool> TryDeletePointOfInterest(int id, int latitude, int longitude, int radius)
        {
                try
                {
                    var toDeletePoint = new PointsOfInterestDTO(id, radius, latitude, longitude, true);

                    dbContext.Update(toDeletePoint);

                    await dbContext.SaveChangesAsync();

                    return true;
                }catch(Exception ex){
                    return false;
                }
        }

        public async Task<bool> TryPostPointOfInterest(int id, int latitude, int longitude, int radius)
        {
            try
            {
                var ToPostPOI = new PointsOfInterestDTO(id, radius, latitude, longitude, false);

                var clientOptions = new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };
                client = new ServiceBusClient("Endpoint=sb://datcservicebusproject.servicebus.windows.net/;SharedAccessKeyName=Admin;SharedAccessKey=N8uTNibOhllgmetihPnl/KGDO9z4D8DcnH4NIxrsXzk=;EntityPath=datcprojectqueue", clientOptions);
                sender = client.CreateSender("datcprojectqueue");

                // create a batch 
                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                string jsonString = JsonConvert.SerializeObject(ToPostPOI);
                messageBatch.TryAddMessage(new ServiceBusMessage(jsonString));

                try
                {
                    await sender.SendMessagesAsync(messageBatch);
                }
                finally
                {
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }
                //Saving into database
                dbContext.Add(ToPostPOI);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

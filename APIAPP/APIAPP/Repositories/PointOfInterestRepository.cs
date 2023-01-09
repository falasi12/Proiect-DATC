using APIAPP.Models;
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

namespace APIAPP.Repositories
{
    public class PointOfInterestRepository : IPointOfInterestRepository
    {
        private readonly InfoContext dbContext;

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
    }
}

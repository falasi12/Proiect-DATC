using APIAPP.Models;
using APIAPP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIAPP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {

        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("userLogin")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult> TryUserLogin([FromServices] IUserRepository userRepository, string UserName, string Password) {
            return Ok(await userRepository.TryLogin(UserName, Password));
        }

        [HttpGet]
        [Route("adminUserLogin")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult> TryAdminUserLogin([FromServices] IUserRepository userRepository, string UserName, string Password)
        {
            return Ok(await userRepository.TryAdminLogin(UserName, Password));
        }


        [HttpGet]
        [Route("deletePointOfInterest")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult> TryDeletePointOfInterest([FromServices] IPointOfInterestRepository pointOfInterestRepository, int id, int radius, int latitude, int longitude) {
            return Ok(await pointOfInterestRepository.TryDeletePointOfInterest(id, latitude, longitude, radius));
        }

        [HttpGet]
        [Route("AllPOI")]
        [ProducesResponseType(typeof(List<PointsOfInterestDTO>), 200)]
        public async Task<ActionResult> TryGetAllPOI([FromServices] IPointOfInterestRepository pointOfInterestRepository) {
            return Ok(await pointOfInterestRepository.TryGetPointsOfInterest());
        }

        [HttpPost]
        [Route("PostPOI")]
        [ProducesResponseType(typeof(List<PointsOfInterestDTO>), 200)]
        public async Task<ActionResult> TryPostPointOfInterest([FromServices] IPointOfInterestRepository pointOfInterestRepository, int id, int radius, int latitude, int longitude)
        {
            return Ok(await pointOfInterestRepository.TryPostPointOfInterest(id,latitude,longitude,radius));
        }

    }
}
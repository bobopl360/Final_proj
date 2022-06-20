using Application_A.BL.Interfaces;
using Application_A.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application_A.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IRabbitMqService _rabbitMq;

        public PersonController(ILogger<PersonController> logger, IRabbitMqService rabbitMq)
        {
            _logger = logger;
            _rabbitMq = rabbitMq;
        }

        [HttpPost("PublshPerson")]
        public async Task<IActionResult> SendPerson([FromBody] Person p)
        {
            await _rabbitMq.PublshPersonRabbitAsync(p);

            return Ok();
        }

    }
}

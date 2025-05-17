using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/Cars")]
    [ApiController]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarsService _service;

        public CarsApiController(ICarsService service)
        {
            _service = service;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<IEnumerable<Cars>> Get()
        {
            var result = await _service.List(1, 10000, null);
            return result.Results;
        }

        // GET api/Cars/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var car = await _service.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        // POST api/Cars
        [HttpPost]
        public async Task<object> Post([FromBody] Cars car)
        {
            await _service.Save(car);
            return Ok(car);
        }

        // PUT api/Cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cars car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            await _service.Save(car);
            return Ok();
        }

        // DELETE api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _service.Get(id);
            if (car == null)
            {
                return NotFound();
            }

            await _service.Delete(id);
            return Ok();
        }
    }
}
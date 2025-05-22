using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/Rents")]
    [ApiController]
    public class RentsApiController : ControllerBase
    {
        private readonly IRentService _service;

        public RentsApiController(IRentService service)
        {
            _service = service;
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<IEnumerable<Rent>> Get()
        {
            var result = await _service.List(1, 10000, null);
            return result.Results;
        }

        // GET api/Rents/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var rent = await _service.Get(id);
            if (rent == null)
            {
                return NotFound();
            }
            return rent;
        }

        // POST api/Rents
        [HttpPost]
        public async Task<object> Post([FromBody] Rent rent)
        {
            await _service.Save(rent);
            return Ok(rent);
        }

        // PUT api/Rents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rent rent)
        {
            if (id != rent.Id)
            {
                return BadRequest();
            }

            await _service.Save(rent);
            return Ok();
        }

        // DELETE api/Rents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rent = await _service.Get(id);
            if (rent == null)
            {
                return NotFound();
            }

            await _service.Delete(id);
            return Ok();
        }
    }
}

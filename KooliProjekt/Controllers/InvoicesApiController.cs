using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/Invoices")]
    [ApiController]
    public class InvoicesApiController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoicesApiController(IInvoiceService service)
        {
            _service = service;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<IEnumerable<Invoice>> Get()
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/Invoices/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var invoice = await _service.Get(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return invoice;
        }

        // POST api/Invoices
        [HttpPost]
        public async Task<object> Post([FromBody] Invoice invoice)
        {
            await _service.Save(invoice);
            return Ok(invoice);
        }

        // PUT api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            await _service.Save(invoice);
            return Ok();
        }

        // DELETE api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _service.Get(id);
            if (invoice == null)
            {
                return NotFound();
            }

            await _service.Delete(id);
            return Ok();
        }
    }
}
